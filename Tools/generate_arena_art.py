"""Generates the arena art into Assets/Art/Arenas/<theme>/: floor and wall tiles (16x16) and props.
Run from the project root: python Tools/generate_arena_art.py"""
import os, random, math
from PIL import Image

ROOT = os.path.join("Assets", "Art", "Arenas")
OUT = (28, 22, 34)


class C:
    def __init__(self, w, h):
        self.w, self.h = w, h
        self.im = Image.new("RGBA", (w, h), (0, 0, 0, 0))
        self.px = self.im.load()

    def set(self, x, y, c):
        if 0 <= x < self.w and 0 <= y < self.h:
            self.px[x, y] = c if len(c) == 4 else tuple(c) + (255,)

    def rect(self, x0, y0, x1, y1, c):
        for y in range(y0, y1 + 1):
            for x in range(x0, x1 + 1):
                self.set(x, y, c)

    def ellipse(self, cx, cy, rx, ry, c):
        for y in range(int(cy - ry) - 1, int(cy + ry) + 2):
            for x in range(int(cx - rx) - 1, int(cx + rx) + 2):
                if ((x + 0.5 - cx) / rx) ** 2 + ((y + 0.5 - cy) / ry) ** 2 <= 1:
                    self.set(x, y, c)

    def shadow(self, cx, cy, rx, ry):
        for y in range(self.h):
            for x in range(self.w):
                if ((x + 0.5 - cx) / rx) ** 2 + ((y + 0.5 - cy) / ry) ** 2 <= 1 and self.px[x, y][3] == 0:
                    self.px[x, y] = (0, 0, 0, 60)

    def outline(self, color=OUT):
        src = self.im.copy().load()
        for y in range(self.h):
            for x in range(self.w):
                if src[x, y][3] > 100:
                    continue
                for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
                    nx, ny = x + dx, y + dy
                    if 0 <= nx < self.w and 0 <= ny < self.h and src[nx, ny][3] > 100:
                        self.px[x, y] = color + (255,)
                        break

    def save(self, theme, name):
        folder = os.path.join(ROOT, theme)
        os.makedirs(folder, exist_ok=True)
        self.im.save(os.path.join(folder, name + ".png"))


def shade(c, d):
    return tuple(max(0, min(255, v + d)) for v in c)


def noisy(base, seed, speck_light, speck_dark, n=26):
    r = random.Random(seed)
    t = C(16, 16)
    t.rect(0, 0, 15, 15, base)
    for _ in range(n):
        t.set(r.randrange(16), r.randrange(16), speck_light if r.random() < 0.5 else speck_dark)
    return t, r


# ------------------------------------------------------------------ cave
def cave():
    base = (62, 56, 74)
    for i in range(4):
        t, r = noisy(base, 10 + i, shade(base, 14), shade(base, -14))
        for _ in range(2):  # small cracks
            x, y = r.randrange(2, 13), r.randrange(2, 13)
            for k in range(r.randrange(2, 5)):
                t.set(x + k, y + (k % 2), shade(base, -26))
        t.save("cave", "floor_%d" % i)
    for i in range(2):
        r = random.Random(40 + i)
        t = C(16, 16)
        t.rect(0, 0, 15, 15, (36, 32, 46))
        for _ in range(5):
            cx, cy = r.randrange(2, 14), r.randrange(2, 14)
            t.ellipse(cx, cy, r.randrange(3, 6), r.randrange(3, 5), (48, 43, 60))
            t.ellipse(cx - 1, cy - 1, 2, 1.5, (66, 60, 80))
        t.save("cave", "wall_%d" % i)

    c = C(16, 32)  # stalagmite
    for y in range(6, 30):
        half = 1 + (y - 6) * 0.28
        c.rect(int(8 - half), y, int(8 + half), y, (96, 88, 112))
        c.rect(int(8 - half), y, int(8 - half) + 1, y, (124, 116, 142))
        c.rect(int(8 + half) - 1, y, int(8 + half), y, (66, 60, 82))
    c.outline(); c.shadow(8, 30, 7, 2); c.save("cave", "stalagmite")

    c = C(16, 16)  # rock
    c.ellipse(8, 10, 6.5, 4.5, (92, 86, 108)); c.ellipse(7, 8, 3.5, 2.5, (120, 112, 138))
    c.outline(); c.save("cave", "rock")

    c = C(16, 24)  # crystal cluster
    for (x, h, col) in ((5, 14, (92, 190, 255)), (9, 20, (140, 220, 255)), (12, 11, (70, 150, 230))):
        for y in range(23 - h, 22):
            half = 1 if y > 23 - h + 3 else 0
            c.rect(x - half, y, x + half, y, col)
        c.set(x, 23 - h, (230, 250, 255))
    c.outline(); c.shadow(8, 22, 7, 2); c.save("cave", "crystal")

    c = C(16, 16)  # glowing mushrooms
    for (x, y, rr) in ((5, 11, 3), (11, 12, 2)):
        c.rect(x, y, x, 14, (210, 205, 190))
        c.ellipse(x, y, rr + 1, rr, (70, 220, 200)); c.set(x - 1, y - 1, (190, 255, 245))
    c.outline(); c.save("cave", "mushrooms")


# ------------------------------------------------------------------ dungeon
def dungeon():
    base = (86, 84, 96)
    for i in range(4):
        t, r = noisy(base, 60 + i, shade(base, 10), shade(base, -10), 14)
        t.rect(0, 0, 15, 0, shade(base, -34)); t.rect(0, 0, 0, 15, shade(base, -34))
        t.rect(1, 1, 14, 1, shade(base, 14)); t.rect(1, 1, 1, 14, shade(base, 14))
        if i % 2: t.set(r.randrange(3, 13), r.randrange(3, 13), (60, 90, 60))
        t.save("dungeon", "floor_%d" % i)
    for i in range(2):
        t = C(16, 16)
        t.rect(0, 0, 15, 15, (54, 50, 64))
        for row in range(4):
            y = row * 4
            t.rect(0, y, 15, y, (30, 27, 38))
            off = 4 if (row + i) % 2 else 0
            for x in range(off, 16, 8):
                t.rect(x % 16, y, x % 16, y + 3, (30, 27, 38))
            t.rect(1, y + 1, 14, y + 1, (74, 70, 86))
        t.save("dungeon", "wall_%d" % i)

    c = C(16, 32)  # pillar
    c.rect(2, 26, 13, 30, (110, 106, 122)); c.rect(2, 3, 13, 6, (126, 122, 138))
    c.rect(4, 7, 11, 26, (118, 114, 130)); c.rect(4, 7, 5, 26, (146, 142, 160)); c.rect(10, 7, 11, 26, (84, 80, 98))
    c.outline(); c.shadow(8, 30, 7, 2); c.save("dungeon", "pillar")

    c = C(16, 32)  # brazier with a flame
    c.rect(7, 18, 8, 28, (70, 64, 78)); c.rect(4, 28, 11, 30, (70, 64, 78))
    c.rect(3, 14, 12, 18, (96, 90, 106)); c.rect(3, 14, 12, 14, (130, 124, 140))
    c.ellipse(8, 9, 3.5, 6, (255, 140, 40)); c.ellipse(8, 10, 2, 4, (255, 215, 90)); c.set(8, 4, (255, 90, 30))
    c.outline(); c.shadow(8, 30, 7, 2); c.save("dungeon", "brazier")

    c = C(16, 16)  # bones
    c.rect(3, 11, 12, 12, (225, 220, 200)); c.set(2, 10, (225, 220, 200)); c.set(13, 13, (225, 220, 200))
    c.ellipse(8, 8, 3.5, 3, (235, 230, 214)); c.set(7, 8, (40, 36, 44)); c.set(9, 8, (40, 36, 44)); c.rect(7, 10, 9, 10, (180, 175, 160))
    c.outline(); c.save("dungeon", "bones")

    c = C(16, 16)  # cracked slab / rubble
    c.ellipse(6, 11, 4, 2.5, (118, 114, 130)); c.ellipse(11, 12, 3, 2, (96, 92, 108)); c.ellipse(8, 9, 2.5, 2, (136, 132, 148))
    c.outline(); c.save("dungeon", "rubble")

    c = C(16, 32)  # hanging banner on a pole
    c.rect(7, 4, 8, 29, (90, 62, 40)); c.rect(3, 4, 12, 5, (110, 78, 50))
    c.rect(4, 6, 11, 20, (150, 34, 40)); c.rect(4, 6, 5, 20, (176, 52, 56))
    c.set(6, 21, (150, 34, 40)); c.set(9, 21, (150, 34, 40)); c.rect(7, 10, 8, 14, (230, 190, 70))
    c.outline(); c.shadow(8, 30, 6, 2); c.save("dungeon", "banner")


# ------------------------------------------------------------------ cornfield
def cornfield():
    base = (122, 92, 56)
    for i in range(4):
        t, r = noisy(base, 80 + i, shade(base, 14), shade(base, -16), 22)
        for y in (3, 9, 14): t.rect(0, y, 15, y, shade(base, -22))      # furrows
        for _ in range(2): t.set(r.randrange(16), r.choice((2, 8, 13)), (96, 150, 60))  # sprouts
        t.save("cornfield", "floor_%d" % i)
    for i in range(2):
        r = random.Random(90 + i)
        t = C(16, 16)
        t.rect(0, 0, 15, 15, (70, 120, 46))
        for _ in range(14):
            x, y = r.randrange(16), r.randrange(16)
            t.rect(x, y, x, min(15, y + 5), (96, 156, 54)); t.set(x, y, (226, 196, 80))
            t.set(min(15, x + 1), y + 2, (52, 96, 40))
        t.save("cornfield", "wall_%d" % i)

    for name, seed in (("corn_a", 1), ("corn_b", 2)):  # a clump of tall corn
        r = random.Random(seed)
        c = C(16, 32)
        for x in (3, 7, 11):
            top = r.randrange(3, 9)
            c.rect(x, top, x + 1, 29, (84, 146, 52)); c.rect(x, top, x, 29, (112, 176, 66))
            c.rect(x - 1, top + 8, x + 2, top + 9, (60, 110, 40))
            c.rect(x, top + 3, x + 1, top + 7, (232, 198, 76))
        c.outline(); c.shadow(8, 30, 7, 2); c.save("cornfield", name)

    c = C(16, 32)  # scarecrow
    c.rect(7, 12, 8, 28, (110, 78, 46)); c.rect(1, 14, 14, 15, (110, 78, 46))
    c.rect(5, 15, 10, 23, (150, 60, 50)); c.ellipse(8, 9, 3.5, 3.5, (216, 184, 110))
    c.rect(4, 5, 11, 6, (80, 56, 36)); c.rect(6, 2, 9, 5, (80, 56, 36)); c.set(7, 9, (30, 26, 30)); c.set(9, 9, (30, 26, 30))
    c.outline(); c.shadow(8, 30, 6, 2); c.save("cornfield", "scarecrow")

    c = C(16, 16)  # hay bale
    c.rect(2, 6, 13, 13, (216, 178, 70)); c.rect(2, 6, 13, 7, (240, 208, 100)); c.rect(2, 13, 13, 13, (170, 132, 50))
    for x in (5, 10): c.rect(x, 6, x, 13, (150, 110, 44))
    c.outline(); c.shadow(8, 14, 7, 2); c.save("cornfield", "hay")

    c = C(16, 16)  # pumpkin
    c.ellipse(8, 10, 5.5, 4.5, (226, 120, 30)); c.rect(6, 6, 6, 14, (176, 82, 24)); c.rect(10, 7, 10, 13, (176, 82, 24))
    c.rect(7, 4, 8, 5, (70, 120, 40)); c.set(6, 8, (250, 170, 80))
    c.outline(); c.save("cornfield", "pumpkin")


# ------------------------------------------------------------------ farm
def farm():
    greens = ((74, 124, 58), (82, 134, 64), (66, 112, 52), (78, 130, 60))
    for i in range(4):
        t, r = noisy(greens[i], 120 + i, (96, 150, 76), (56, 98, 46), 28)
        if i == 3:
            t.ellipse(8, 8, 5, 3.5, (150, 112, 70)); t.ellipse(7, 7, 3, 2, (170, 130, 84))
        t.save("farm", "floor_%d" % i)
    for i in range(2):
        r = random.Random(130 + i)
        t = C(16, 16)
        t.rect(0, 0, 15, 15, (44, 88, 42))
        for _ in range(7):
            cx, cy = r.randrange(1, 15), r.randrange(1, 15)
            t.ellipse(cx, cy, r.randrange(3, 5), r.randrange(3, 5), (56, 108, 50)); t.ellipse(cx - 1, cy - 1, 1.5, 1.5, (86, 146, 66))
        t.save("farm", "wall_%d" % i)

    c = C(16, 16)  # hay bale (round)
    c.ellipse(8, 9, 6, 5, (216, 178, 70)); c.ellipse(8, 9, 3.5, 3, (240, 208, 100)); c.set(8, 9, (170, 132, 50))
    c.outline(); c.shadow(8, 14, 7, 2); c.save("farm", "hay_round")

    c = C(16, 16)  # water trough
    c.rect(1, 7, 14, 13, (110, 78, 46)); c.rect(2, 8, 13, 11, (66, 130, 190)); c.rect(2, 8, 13, 8, (120, 180, 230))
    c.rect(1, 13, 14, 13, (80, 56, 34))
    c.outline(); c.shadow(8, 14, 7, 2); c.save("farm", "trough")

    c = C(16, 16)  # wheelbarrow-ish cart: a crate with wheel
    c.rect(2, 5, 12, 11, (140, 100, 60)); c.rect(2, 5, 12, 6, (170, 126, 78)); c.ellipse(11, 12, 3, 3, (60, 44, 34)); c.set(11, 12, (150, 120, 90))
    c.outline(); c.shadow(8, 14, 7, 2); c.save("farm", "cart")

    c = C(64, 52)  # barn (top rows stay free)
    c.rect(3, 4, 60, 18, (150, 40, 40)); c.rect(3, 4, 60, 6, (190, 70, 60))
    for x in range(6, 60, 6): c.rect(x, 7, x, 17, (126, 32, 34))
    c.rect(3, 18, 60, 47, (170, 48, 46)); c.rect(3, 18, 60, 19, (120, 30, 32))
    for x in range(4, 60, 5): c.rect(x, 20, x, 46, (146, 38, 40))
    c.rect(22, 28, 41, 47, (222, 214, 196)); c.rect(22, 28, 41, 29, (140, 100, 64))
    c.rect(22, 28, 22, 47, (140, 100, 64)); c.rect(41, 28, 41, 47, (140, 100, 64)); c.rect(31, 29, 32, 47, (140, 100, 64))
    for k in range(10): c.set(23 + k, 30 + k * 1, (140, 100, 64)); c.set(40 - k, 30 + k * 1, (140, 100, 64))
    c.rect(26, 10, 37, 18, (46, 30, 28)); c.rect(26, 10, 37, 10, (222, 214, 196))
    c.rect(3, 47, 60, 49, (96, 90, 84))
    c.outline(); c.save("farm", "barn")

    # tall corn plant (16x32, planted in rows) and a furrow tile to put under the rows
    c = C(16, 32)
    r = random.Random(77)
    c.rect(7, 8, 8, 29, (70, 130, 52)); c.rect(7, 8, 7, 29, (92, 156, 66))
    for y, side in ((26, -1), (21, 1), (16, -1), (11, 1)):
        for k in range(6):
            x = 8 + side * (k + 1); yy = y - (k // 2) + (1 if k > 3 else 0)
            c.set(x, yy, (60, 120, 46)); c.set(x, yy + 1, (84, 146, 60))
    for y, side in ((20, -1), (14, 1)):
        c.rect(8 + side * 2 - (1 if side < 0 else 0), y, 8 + side * 2 + (0 if side < 0 else 1), y + 4, (232, 184, 56))
        c.rect(8 + side * 2 - (1 if side < 0 else 0), y + 3, 8 + side * 2 + (0 if side < 0 else 1), y + 4, (250, 222, 110))
    c.rect(6, 5, 9, 8, (214, 190, 80)); c.rect(7, 2, 8, 4, (196, 164, 60)); c.set(7, 1, (170, 140, 50)); c.set(8, 0, (150, 120, 44))
    c.outline(); c.shadow(8, 30, 6, 2); c.save("farm", "corn_row")

    t = C(16, 16)
    t.rect(0, 0, 15, 15, (120, 84, 54))
    for y in (2, 7, 12):
        t.rect(0, y, 15, y + 1, (92, 62, 40)); t.rect(0, y + 2, 15, y + 2, (140, 100, 66))
    r = random.Random(90)
    for _ in range(10): t.set(r.randrange(16), r.randrange(16), (150, 112, 76))
    t.save("farm", "furrow")


for fn in (cave, dungeon, cornfield, farm):
    fn()
print("arena art written to", ROOT)
