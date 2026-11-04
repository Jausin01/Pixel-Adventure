"""Generates Assets/Art/World/portal.png: a stone gate with an oblique 3D look and a glowing rift inside.
Run: python Tools/generate_portal.py"""
from PIL import Image
import math

W, H = 48, 56
OUT = (30, 24, 40, 255)
im = Image.new("RGBA", (W, H), (0, 0, 0, 0))
px = im.load()

def put(x, y, c):
    if 0 <= x < W and 0 <= y < H:
        px[x, y] = c if len(c) == 4 else c + (255,)

def rect(x0, y0, x1, y1, c):
    for y in range(y0, y1 + 1):
        for x in range(x0, x1 + 1):
            put(x, y, c)

LIGHT, MID, DARK, DEEP = (196, 196, 210), (150, 150, 168), (104, 104, 126), (70, 70, 92)

# ground shadow
for y in range(48, 56):
    for x in range(0, 48):
        dx, dy = (x - 24.0) / 24.0, (y - 52.0) / 4.0
        if dx * dx + dy * dy <= 1: put(x, y, (0, 0, 0, 70))

# base slab: lit top face, front face, dark right face
rect(3, 42, 44, 46, LIGHT)
rect(2, 47, 43, 52, MID)
rect(44, 43, 46, 51, DARK)
rect(2, 47, 43, 47, LIGHT)
rect(2, 52, 43, 52, DEEP)

# rift interior
for y in range(12, 43):
    for x in range(15, 33):
        d = math.hypot((x - 24) / 9.0, (y - 27) / 15.5)
        t = min(1.0, d)
        c = (int(150 - 90 * t), int(90 - 40 * t), int(255 - 40 * t))
        ring = int(d * 7) % 2 == 0 and d < 0.9
        if ring: c = (min(255, c[0] + 40), min(255, c[1] + 50), 255)
        put(x, y, c)
for (x, y) in [(24, 20), (20, 28), (28, 33), (23, 38), (26, 15), (18, 36), (30, 24)]:
    put(x, y, (235, 220, 255))

# pillars: front face + dark right face + cap
for x0 in (8, 31):
    rect(x0, 14, x0 + 7, 43, MID)
    rect(x0, 14, x0 + 1, 43, LIGHT)
    rect(x0 + 8, 12, x0 + 10, 41, DARK)
    rect(x0 - 1, 11, x0 + 8, 13, LIGHT)
    for y in range(20, 42, 7): rect(x0 + 2, y, x0 + 7, y, DARK)

# lintel with lit top
rect(6, 7, 41, 13, MID)
rect(6, 7, 41, 8, LIGHT)
rect(42, 5, 44, 11, DARK)
for i in range(3): rect(6 + i, 6 - i, 41 + i, 6 - i, LIGHT)
rect(22, 9, 26, 12, (120, 90, 230))  # keystone gem

src = im.copy().load()
for y in range(H):
    for x in range(W):
        if src[x, y][3] > 100: continue
        for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
            nx, ny = x + dx, y + dy
            if 0 <= nx < W and 0 <= ny < H and src[nx, ny][3] > 100:
                put(x, y, OUT); break

im.save("Assets/Art/World/portal.png")
