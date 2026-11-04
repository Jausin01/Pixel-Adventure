"""Generates the pixel art of the town: ground tiles, buildings and props.

Run from the project root:  python Tools/generate_town_art.py
Writes PNG files to Assets/Art/Tiles and Assets/Art/Town (16 pixels per unit, transparent background).
Edit the palettes and drawing functions to restyle the town, then reimport the art in Unity.
"""
import os
import random
from PIL import Image

TILES = "Assets/Art/Tiles"
TOWN = "Assets/Art/Town"

# ---- palette (matches the grass of Assets/Art/World/ground.png) ----
GRASS = [(74, 124, 58), (82, 134, 64), (66, 112, 52), (110, 160, 80)]
OUTLINE = (40, 28, 28)
WOOD = [(176, 128, 82), (140, 98, 60), (104, 72, 44)]
STONE = [(152, 152, 164), (116, 116, 130), (86, 86, 100)]
ROOF_RED = [(176, 64, 52), (136, 44, 40), (206, 98, 74)]
ROOF_BLUE = [(72, 104, 176), (50, 74, 134), (114, 146, 206)]
ROOF_BROWN = [(124, 84, 56), (92, 60, 40), (156, 112, 76)]
ROOF_SLATE = [(84, 90, 108), (60, 64, 80), (112, 120, 140)]
GLASS = (244, 214, 116)
GOLD = (240, 200, 70)


class Canvas:
    def __init__(self, w, h):
        self.im = Image.new("RGBA", (w, h), (0, 0, 0, 0))
        self.px = self.im.load()
        self.w, self.h = w, h

    def set(self, x, y, c):
        if 0 <= x < self.w and 0 <= y < self.h:
            self.px[x, y] = c if len(c) == 4 else c + (255,)

    def rect(self, x0, y0, x1, y1, c):
        for y in range(y0, y1 + 1):
            for x in range(x0, x1 + 1):
                self.set(x, y, c)

    def save(self, folder, name):
        os.makedirs(folder, exist_ok=True)
        self.im.save(os.path.join(folder, name + ".png"))

    def outline(self, color=OUTLINE):
        """Adds a 1 pixel outline around every opaque pixel."""
        src = self.im.copy().load()
        for y in range(self.h):
            for x in range(self.w):
                if src[x, y][3] != 0:
                    continue
                for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
                    nx, ny = x + dx, y + dy
                    if 0 <= nx < self.w and 0 <= ny < self.h and src[nx, ny][3] != 0:
                        self.px[x, y] = color + (255,)
                        break


# ------------------------------------------------------------------ tiles
def grass_tile(seed):
    rnd = random.Random(seed)
    c = Canvas(16, 16)
    for y in range(16):
        for x in range(16):
            r = rnd.random()
            col = GRASS[0]
            if r < 0.14:
                col = GRASS[1]
            elif r < 0.22:
                col = GRASS[2]
            c.set(x, y, col)
    if seed % 2 == 1:  # a few tufts
        for _ in range(2):
            x, y = rnd.randrange(1, 15), rnd.randrange(2, 15)
            c.set(x, y, GRASS[3])
            c.set(x, y - 1, GRASS[3])
    return c


def cobble_tile(seed, base, mortar, light, dark=None):
    """Irregular cobblestones: 4x4-ish stones separated by mortar lines."""
    rnd = random.Random(seed)
    c = Canvas(16, 16)
    c.rect(0, 0, 15, 15, mortar)
    rows = [(0, 3), (4, 7), (8, 11), (12, 15)]
    for i, (y0, y1) in enumerate(rows):
        x = -(rnd.randrange(0, 3) if i % 2 else 0)
        while x < 16:
            width = rnd.choice([4, 5, 5, 6])
            for yy in range(y0, y1):
                for xx in range(x + 1, x + width):
                    shade = base
                    if yy == y0:
                        shade = light
                    elif rnd.random() < 0.1:
                        shade = dark or base
                    c.set(xx, yy, shade)
            x += width
    return c


def build_tiles():
    for i in range(4):
        grass_tile(i * 7 + 3).save(TILES, "grass_%d" % i)
    cobble_tile(1, (156, 150, 142), (112, 106, 100), (182, 176, 166), (140, 134, 128)).save(TILES, "plaza_0")
    cobble_tile(2, (150, 146, 140), (112, 106, 100), (178, 172, 164), (136, 130, 126)).save(TILES, "plaza_1")
    cobble_tile(3, (118, 112, 118), (80, 76, 86), (146, 140, 148), (104, 98, 106)).save(TILES, "plaza_rim")
    cobble_tile(4, (170, 138, 98), (126, 98, 70), (194, 164, 120), (156, 124, 88)).save(TILES, "road_0")
    cobble_tile(5, (164, 132, 94), (126, 98, 70), (190, 160, 116), (150, 120, 84)).save(TILES, "road_1")


# ------------------------------------------------------------------ buildings
OY = 4  # free rows at the top of a building for chimneys, flags and smoke


def roof(c, cols, top, bottom, left, right):
    """Pitched roof seen from the front: a trapezoid with shingle lines."""
    base, dark, light = cols
    height = bottom - top
    for y in range(top, bottom + 1):
        inset = round(10 * (1 - (y - top) / float(height)))
        for x in range(left + inset, right - inset + 1):
            col = base
            if (y - top) % 4 == 3:
                col = dark
            elif ((x + (y - top) // 4 * 3) % 7 == 0):
                col = light
            c.set(x, y, col)
        c.set(left + inset, y, light)
    for x in range(left + 10, right - 9):
        c.set(x, top, light)
    for x in range(left, right + 1):  # shadow under the eaves
        c.set(x, bottom, dark)


def wall(c, cols, top, bottom, left, right, planks=True):
    base, dark, deeper = cols
    c.rect(left, top, right, bottom, base)
    if planks:
        for x in range(left + 2, right, 5):
            for y in range(top, bottom + 1):
                c.set(x, y, dark)
    for x in range(left, right + 1):  # shade below the roof
        c.set(x, top, deeper)
        c.set(x, top + 1, dark)
    for y in range(top, bottom + 1):
        c.set(left, y, deeper)
        c.set(right, y, deeper)


def foundation(c, top, bottom, left, right):
    c.rect(left, top, right, bottom, STONE[0])
    for y in range(top, bottom + 1, 3):
        for x in range(left, right + 1):
            c.set(x, y, STONE[1])
    for y in range(top, bottom + 1, 3):
        for x in range(left + (y // 3 % 2) * 4, right + 1, 8):
            for yy in range(y, min(y + 3, bottom + 1)):
                c.set(x, yy, STONE[1])


def door(c, x0, y0, x1, y1, cols=(((120, 80, 48)), (92, 60, 36))):
    base, dark = cols
    c.rect(x0 - 1, y0 - 1, x1 + 1, y1, (60, 40, 28))
    c.rect(x0, y0, x1, y1, base)
    for x in range(x0 + 2, x1, 4):
        for y in range(y0, y1 + 1):
            c.set(x, y, dark)
    c.set(x0, y0, (60, 40, 28))
    c.set(x1, y0, (60, 40, 28))


def window(c, x0, y0, w=10, h=9):
    c.rect(x0 - 1, y0 - 1, x0 + w, y0 + h, (60, 40, 28))
    c.rect(x0, y0, x0 + w - 1, y0 + h - 1, GLASS)
    c.rect(x0 + w // 2 - 1, y0, x0 + w // 2, y0 + h - 1, (60, 40, 28))
    c.rect(x0, y0 + h // 2 - 1, x0 + w - 1, y0 + h // 2, (60, 40, 28))
    for x in range(x0, x0 + w // 2 - 1):
        c.set(x, y0, (255, 240, 170))


def building_base(roof_cols, wall_cols, planks=True):
    c = Canvas(64, 52)
    roof(c, roof_cols, OY, OY + 19, 0, 63)
    wall(c, wall_cols, OY + 20, 51, 4, 59, planks)
    foundation(c, 46, 51, 4, 59)
    return c


def guild_hall():
    c = building_base(ROOF_BLUE, WOOD)
    door(c, 26, 32, 37, 49)
    window(c, 9, 32)
    window(c, 46, 32)
    for y in range(0, OY + 9):  # flag on a pole
        c.set(31, y, (70, 50, 40))
    c.rect(32, 0, 38, 4, GOLD)
    c.rect(32, 1, 36, 3, (230, 60, 60))
    c.outline()
    return c


def dojo():
    c = building_base(ROOF_RED, ((226, 214, 178), (186, 170, 130), (120, 90, 60)), planks=False)
    for x in range(6, 59, 13):  # timber frame over paper walls
        for y in range(OY + 22, 46):
            c.set(x, y, (92, 60, 40))
    for y in (OY + 28, 41):
        for x in range(5, 59):
            c.set(x, y, (92, 60, 40))
    door(c, 26, 32, 37, 49, ((196, 170, 120), (150, 120, 80)))
    for y in range(OY + 20, 33):  # banner with crossed swords next to the door
        for x in range(10, 17):
            c.set(x, y, (176, 48, 44))
    for i in range(5):
        c.set(11 + i, OY + 22 + i, (230, 230, 240))
        c.set(15 - i, OY + 22 + i, (230, 230, 240))
    for x in (2, 3, 4, 59, 60, 61):  # gold ridge ornaments and lifted eave corners
        c.set(x, OY + 17, ROOF_RED[2])
    c.rect(26, OY - 3, 29, OY, GOLD)
    c.rect(34, OY - 3, 37, OY, GOLD)
    c.outline()
    return c


def forge():
    c = building_base(ROOF_SLATE, ((140, 138, 150), (108, 106, 120), (78, 76, 92)), planks=False)
    for y in range(OY + 22, 46, 4):  # stone courses
        for x in range(5, 59):
            c.set(x, y, (108, 106, 120))
    c.rect(44, 0, 51, OY + 14, (120, 118, 132))  # chimney
    c.rect(44, 0, 45, OY + 14, (150, 148, 160))
    c.rect(44, 0, 51, 1, (70, 68, 82))
    c.rect(18, 29, 45, 49, (50, 34, 28))  # open furnace front
    c.rect(20, 31, 43, 49, (30, 22, 20))
    c.rect(24, 40, 39, 49, (255, 140, 40))
    c.rect(27, 43, 36, 49, (255, 210, 90))
    c.rect(29, 46, 34, 49, (255, 245, 170))
    window(c, 7, 32, 8, 8)
    c.outline()
    return c


def storehouse():
    c = building_base(ROOF_BROWN, ((158, 112, 70), (124, 86, 52), (90, 62, 40)))
    c.rect(18, 30, 45, 49, (60, 40, 28))
    c.rect(20, 32, 31, 49, (122, 84, 52))
    c.rect(33, 32, 44, 49, (122, 84, 52))
    for x in range(22, 31, 3):
        for y in range(32, 50):
            c.set(x, y, (96, 64, 40))
    for x in range(35, 44, 3):
        for y in range(32, 50):
            c.set(x, y, (96, 64, 40))
    for i in range(10):  # braces
        c.set(21 + i, 33 + i * 1, (80, 52, 34))
        c.set(30 - i, 33 + i * 1, (80, 52, 34))
        c.set(34 + i, 33 + i, (80, 52, 34))
        c.set(43 - i, 33 + i, (80, 52, 34))
    c.rect(32, 40, 33, 42, GOLD)
    c.rect(8, OY + 24, 14, OY + 30, (206, 180, 90))  # hay at the side
    c.outline()
    return c


def market():
    c = Canvas(64, 52)
    # back wall and shelves
    c.rect(6, 20, 57, 40, (120, 84, 52))
    for y in (26, 33):
        c.rect(6, y, 57, y + 1, (86, 58, 36))
    for x, col in ((10, (220, 70, 60)), (16, (240, 160, 50)), (22, (110, 190, 90)), (46, (90, 150, 230)), (52, (190, 110, 220))):
        c.rect(x, 22, x + 3, 25, col)
    for x, col in ((12, (240, 220, 120)), (30, (220, 70, 60)), (38, (110, 190, 90)), (50, (240, 160, 50))):
        c.rect(x, 29, x + 3, 32, col)
    # posts
    c.rect(5, 16, 8, 50, WOOD[1])
    c.rect(55, 16, 58, 50, WOOD[1])
    # counter
    c.rect(4, 40, 59, 51, WOOD[0])
    for x in range(7, 59, 5):
        for y in range(42, 52):
            c.set(x, y, WOOD[1])
    c.rect(4, 40, 59, 41, WOOD[2])
    for i, (x, col) in enumerate(((12, (220, 70, 60)), (18, (240, 160, 50)), (24, (220, 70, 60)), (42, (110, 190, 90)), (48, (240, 160, 50)))):
        c.rect(x, 36, x + 4, 39, col)
        c.set(x + 1, 36, (255, 255, 255))
    # striped awning with scalloped edge
    for y in range(OY, 19):
        inset = round(5 * (1 - (y - OY) / 14.0))
        for x in range(1 + inset, 63 - inset):
            c.set(x, y, (232, 232, 232) if (x // 6) % 2 == 0 else (200, 60, 52))
    for x in range(1, 63):
        if (x // 6) % 2 == 0:
            for y in (19, 20):
                c.set(x, y, (232, 232, 232))
        else:
            for y in (19, 20):
                c.set(x, y, (200, 60, 52))
        if x % 6 in (0, 5):
            c.set(x, 20, (0, 0, 0, 0))
    c.outline()
    return c


# ------------------------------------------------------------------ props
def bush(seed):
    rnd = random.Random(seed)
    c = Canvas(16, 16)
    for y in range(5, 15):
        for x in range(1, 15):
            dx, dy = (x - 7.5) / 6.5, (y - 9.5) / 5.0
            if dx * dx + dy * dy <= 1.0:
                c.set(x, y, (52, 108, 46))
    for _ in range(26):
        x, y = rnd.randrange(2, 14), rnd.randrange(5, 12)
        if c.px[x, y][3]:
            c.set(x, y, (86, 148, 66))
    for _ in range(12):
        x, y = rnd.randrange(2, 14), rnd.randrange(9, 14)
        if c.px[x, y][3]:
            c.set(x, y, (36, 80, 36))
    c.outline((30, 62, 32))
    return c


def flowers(petal, seed):
    rnd = random.Random(seed)
    c = Canvas(16, 16)
    for _ in range(5):
        x, y = rnd.randrange(2, 14), rnd.randrange(6, 14)
        c.set(x, y, (60, 104, 46))
        c.set(x, y + 1, (60, 104, 46))
        c.set(x, y - 1, petal)
        c.set(x - 1, y - 1, petal[:3])
        c.set(x + 1, y - 1, (255, 250, 220))
    return c


def rock():
    c = Canvas(16, 16)
    for y in range(7, 14):
        for x in range(2, 14):
            dx, dy = (x - 7.5) / 6.0, (y - 10.5) / 3.6
            if dx * dx + dy * dy <= 1.0:
                c.set(x, y, STONE[0] if y < 11 else STONE[1])
    c.set(5, 8, (190, 190, 200))
    c.set(6, 8, (190, 190, 200))
    c.outline()
    return c


def barrel():
    c = Canvas(16, 16)
    for y in range(3, 15):
        w = 5 if y in (3, 14) else 6
        for x in range(8 - w, 8 + w):
            c.set(x, y, WOOD[0] if x % 3 else WOOD[1])
    for y in (5, 12):
        for x in range(2, 14):
            c.set(x, y, STONE[1])
    c.outline()
    return c


def crate():
    c = Canvas(16, 16)
    c.rect(2, 4, 13, 14, WOOD[0])
    c.rect(2, 4, 13, 5, WOOD[2])
    c.rect(2, 13, 13, 14, WOOD[2])
    c.rect(2, 4, 3, 14, WOOD[2])
    c.rect(12, 4, 13, 14, WOOD[2])
    for i in range(8):
        c.set(4 + i, 6 + i, WOOD[1])
    c.outline()
    return c


def lamp():
    c = Canvas(16, 32)
    c.rect(7, 10, 8, 30, (70, 70, 84))
    c.rect(5, 29, 10, 31, (54, 54, 66))
    c.rect(4, 3, 11, 10, (60, 60, 74))
    c.rect(5, 4, 10, 9, GLASS)
    c.rect(6, 5, 8, 7, (255, 246, 190))
    c.rect(3, 2, 12, 3, (54, 54, 66))
    c.set(7, 1, (54, 54, 66))
    c.set(8, 1, (54, 54, 66))
    c.outline()
    return c


def fence_h():
    c = Canvas(16, 16)
    c.rect(0, 8, 15, 9, WOOD[1])
    c.rect(0, 11, 15, 12, WOOD[1])
    for x in (1, 9):
        c.rect(x, 5, x + 2, 14, WOOD[0])
        c.rect(x, 5, x + 2, 5, WOOD[2])
    c.outline()
    return c


def fence_v():
    c = Canvas(16, 16)
    c.rect(6, 0, 7, 15, WOOD[1])
    c.rect(4, 0, 9, 3, WOOD[0])
    c.rect(4, 8, 9, 11, WOOD[0])
    c.rect(4, 0, 9, 0, WOOD[2])
    c.outline()
    return c


def anvil():
    c = Canvas(16, 16)
    c.rect(5, 11, 10, 14, WOOD[1])  # stump
    c.rect(2, 6, 13, 8, (84, 88, 104))
    c.rect(4, 8, 11, 10, (70, 74, 90))
    c.rect(1, 6, 3, 7, (84, 88, 104))
    c.rect(3, 6, 12, 6, (130, 136, 152))
    c.outline()
    return c


def build_town():
    guild_hall().save(TOWN, "guild_hall")
    dojo().save(TOWN, "dojo")
    forge().save(TOWN, "forge")
    storehouse().save(TOWN, "storehouse")
    market().save(TOWN, "market")
    bush(1).save(TOWN, "bush_0")
    bush(2).save(TOWN, "bush_1")
    flowers((230, 70, 70), 1).save(TOWN, "flowers_red")
    flowers((240, 210, 70), 2).save(TOWN, "flowers_yellow")
    flowers((90, 140, 240), 3).save(TOWN, "flowers_blue")
    rock().save(TOWN, "rock")
    barrel().save(TOWN, "barrel")
    crate().save(TOWN, "crate")
    lamp().save(TOWN, "lamp")
    fence_h().save(TOWN, "fence_h")
    fence_v().save(TOWN, "fence_v")
    anvil().save(TOWN, "anvil")


if __name__ == "__main__":
    build_tiles()
    build_town()
    print("town art written")
