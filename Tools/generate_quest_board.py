"""Generates Assets/Art/World/quest_board.png: a quest board with an oblique 3D look
(front face, lit top face, dark side face, posts and a ground shadow). Run: python Tools/generate_quest_board.py"""
from PIL import Image

W, H = 32, 40
OUT = (40, 26, 20, 255)
im = Image.new("RGBA", (W, H), (0, 0, 0, 0))
px = im.load()

def put(x, y, c):
    if 0 <= x < W and 0 <= y < H:
        px[x, y] = c if len(c) == 4 else c + (255,)

def rect(x0, y0, x1, y1, c):
    for y in range(y0, y1 + 1):
        for x in range(x0, x1 + 1):
            put(x, y, c)

# ground shadow
for y in range(34, 40):
    for x in range(2, 31):
        dx, dy = (x - 16.0) / 14.5, (y - 37.0) / 3.2
        if dx * dx + dy * dy <= 1: put(x, y, (0, 0, 0, 70))

WOOD, WOOD_D, WOOD_L = (150, 104, 62), (104, 70, 42), (190, 142, 92)
SIDE = (88, 58, 36)

# posts (front legs) and their dark side faces
rect(5, 28, 8, 36, WOOD_D); rect(9, 28, 10, 35, SIDE)
rect(22, 28, 25, 36, WOOD_D); rect(26, 28, 27, 35, SIDE)

# side face (depth, shifted up-right) and top face
for i in range(4):
    rect(27 + i, 6 - i, 27 + i, 27 - i, SIDE)
for i in range(4):
    rect(2 + i, 5 - i, 27 + i, 5 - i, WOOD_L)

# front frame and cork
rect(2, 6, 27, 28, WOOD)
rect(2, 6, 27, 7, WOOD_L)
rect(2, 27, 27, 28, WOOD_D)
rect(4, 9, 25, 25, (196, 160, 110))
rect(4, 9, 25, 9, (160, 124, 84))
rect(4, 9, 4, 25, (170, 134, 92))

# notes: paper, a 1 px shadow below and a pin
def note(x, y, w, h, paper=(238, 228, 200)):
    rect(x + 1, y + 1, x + w, y + h, (140, 104, 70))
    rect(x, y, x + w - 1, y + h - 1, paper)
    put(x + w // 2, y, (200, 50, 50))
    for ly in range(y + 3, y + h - 1, 2):
        rect(x + 2, ly, x + w - 3, ly, (150, 136, 112))

note(6, 11, 6, 7); note(14, 12, 5, 6); note(21, 11, 4, 8)
note(6, 20, 5, 4); note(20, 21, 5, 3)
# the highlighted quest note with a red exclamation mark
rect(13, 19, 19, 25, (140, 104, 70)); rect(12, 18, 18, 24, (246, 236, 208))
rect(15, 19, 15, 22, (205, 50, 50)); put(15, 23, (205, 50, 50)); put(15, 19, (205, 50, 50))
put(15, 18, (200, 50, 50))

# outline
src = im.copy().load()
for y in range(H):
    for x in range(W):
        if src[x, y][3] > 100: continue
        for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
            nx, ny = x + dx, y + dy
            if 0 <= nx < W and 0 <= ny < H and src[nx, ny][3] > 100:
                put(x, y, OUT); break

im.save("Assets/Art/World/quest_board.png")
