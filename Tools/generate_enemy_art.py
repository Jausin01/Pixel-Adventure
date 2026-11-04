"""Generates enemy walk frames (3 each, 16x16, facing right, feet on the bottom row) into Assets/Art/Enemies
and their attack effect sprites into Assets/Art/Effects. Run from the project root: python Tools/generate_enemy_art.py"""
import os
from PIL import Image

ENEMIES = os.path.join("Assets", "Art", "Enemies")
EFFECTS = os.path.join("Assets", "Art", "Effects")
OUT = (26, 20, 30)


class C:
    def __init__(self):
        self.im = Image.new("RGBA", (16, 16), (0, 0, 0, 0))
        self.px = self.im.load()

    def set(self, x, y, c):
        if 0 <= x < 16 and 0 <= y < 16:
            self.px[x, y] = c if len(c) == 4 else tuple(c) + (255,)

    def rect(self, x0, y0, x1, y1, c):
        for y in range(y0, y1 + 1):
            for x in range(x0, x1 + 1):
                self.set(x, y, c)

    def ellipse(self, cx, cy, rx, ry, c):
        for y in range(16):
            for x in range(16):
                if ((x + 0.5 - cx) / rx) ** 2 + ((y + 0.5 - cy) / ry) ** 2 <= 1:
                    self.set(x, y, c)

    def line(self, x0, y0, x1, y1, c):
        n = max(abs(x1 - x0), abs(y1 - y0), 1)
        for i in range(n + 1):
            self.set(round(x0 + (x1 - x0) * i / n), round(y0 + (y1 - y0) * i / n), c)

    def outline(self):
        src = self.im.copy().load()
        for y in range(16):
            for x in range(16):
                if src[x, y][3] > 100:
                    continue
                for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
                    nx, ny = x + dx, y + dy
                    if 0 <= nx < 16 and 0 <= ny < 16 and src[nx, ny][3] > 100:
                        self.px[x, y] = OUT + (255,)
                        break

    def save(self, folder, name):
        self.outline()
        os.makedirs(folder, exist_ok=True)
        self.im.save(os.path.join(folder, name + ".png"))


def zombie(i):
    c = C()
    bob = (0, 1, 0)[i]
    skin, dark, shirt, pants = (118, 170, 98), (84, 130, 74), (96, 82, 126), (72, 66, 58)
    legs = ((5, 9), (7, 8), (6, 10))[i]
    for lx in legs:
        c.rect(lx, 12 + bob, lx + 1, 15, pants)
    c.rect(5, 6 + bob, 9, 11 + bob, shirt); c.rect(5, 10 + bob, 9, 11 + bob, dark)        # torso with a torn hem
    c.rect(9, 7 + bob, 14, 8 + bob, skin); c.set(14, 9 + bob, skin)                       # arms stretched forward
    c.rect(9, 7 + bob, 14, 7 + bob, dark)
    c.rect(6, 1 + bob, 11, 5 + bob, skin); c.rect(6, 1 + bob, 11, 1 + bob, dark)          # head
    c.set(10, 3 + bob, (255, 255, 255)); c.set(11, 3 + bob, (190, 40, 40)); c.set(8, 5 + bob, (60, 40, 40))
    c.save(ENEMIES, "zombie_%d" % i)


def skeleton(i):
    c = C()
    bone, shade, socket = (232, 226, 206), (170, 164, 146), (28, 24, 34)
    bob = (0, 1, 0)[i]
    legs = ((6, 10), (8, 8), (7, 10))[i]
    for lx in legs:
        c.rect(lx, 11 + bob, lx, 15, bone); c.set(lx + 1, 15, shade)
    c.rect(7, 7 + bob, 8, 10 + bob, shade)                                                # spine
    for y in (7, 9):
        c.rect(5, y + bob, 10, y + bob, bone)                                             # ribs
    c.rect(10, 8 + bob, 13, 8 + bob, bone); c.rect(13, 6 + bob, 13, 8 + bob, (150, 130, 110))  # arm with a rusty blade
    c.set(13, 5 + bob, (190, 175, 150))
    c.rect(5, 1 + bob, 10, 5 + bob, bone); c.rect(6, 6 + bob, 9, 6 + bob, shade)          # skull and jaw
    c.rect(8, 3 + bob, 9, 4 + bob, socket); c.set(6, 3 + bob, socket)
    c.save(ENEMIES, "skeleton_%d" % i)


def slime(i):
    c = C()
    body, dark, light = (92, 204, 92), (58, 150, 72), (176, 244, 156)
    rx, ry = ((7, 3.6), (6, 4.8), (5, 6))[i]
    cy = 16 - ry
    c.ellipse(8, cy, rx, ry, body)
    c.rect(int(8 - rx) + 1, 15, int(8 + rx) - 1, 15, dark)
    c.ellipse(6, cy - ry * 0.4, rx * 0.35, ry * 0.25, light)
    ey = int(cy - 0.5)
    c.rect(6, ey, 6, ey + 1, (24, 40, 30)); c.rect(10, ey, 10, ey + 1, (24, 40, 30))
    c.save(ENEMIES, "slime_%d" % i)


def spider(i):
    c = C()
    body, leg = (78, 56, 104), (52, 36, 72)
    lift = ((0, 1, 0, 1), (1, 0, 1, 0), (0, 0, 1, 1))[i]
    for k, x in enumerate((5, 7, 9, 11)):
        up = lift[k]
        c.line(x, 10, x - 2, 14 - up, leg); c.line(x, 10, x + 1, 15 - up, leg)
    c.ellipse(7, 9, 4, 3, body); c.ellipse(12, 9, 2.5, 2.2, body)
    c.set(13, 8, (230, 50, 50)); c.set(14, 9, (230, 50, 50)); c.set(6, 7, (130, 100, 170))
    c.save(ENEMIES, "spider_%d" % i)


def claws(name, color):
    c = C()
    for dx in (0, 5):
        c.line(3 + dx, 2, 6 + dx, 7, color); c.line(6 + dx, 7, 3 + dx, 13, color)
        c.set(4 + dx, 2, color)
    c.im.save(os.path.join(EFFECTS, name + ".png"))


def drips():
    c = C()
    col, dark = (104, 214, 100), (60, 150, 74)
    for x in (3, 7, 11):
        c.rect(x, 7, x + 2, 8, col)
        c.rect(x, 9, x + 2, 10, col); c.set(x + 1, 11, col); c.set(x + 1, 12, dark)
    c.im.save(os.path.join(EFFECTS, "slime_drips.png"))


for i in range(3):
    zombie(i); skeleton(i); slime(i); spider(i)
claws("claws_green", (110, 190, 80))
claws("claws_bone", (236, 230, 208))
drips()
print("enemy art written")
