"""Generates the doctor, the save crystal and the records monument (16x16) in Assets/Art/World/.
Run from the project root: python Tools/generate_town_art2.py"""
import os
from PIL import Image

OUT = (28, 22, 34, 255)

def sprite():
    im = Image.new("RGBA", (16, 16), (0, 0, 0, 0))
    return im, im.load()

def rect(px, x0, y0, x1, y1, c):
    for y in range(y0, y1 + 1):
        for x in range(x0, x1 + 1):
            px[x, y] = c + (255,)

def outline(im, px):
    src = im.copy().load()
    for y in range(16):
        for x in range(16):
            if src[x, y][3] > 100:
                continue
            for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
                nx, ny = x + dx, y + dy
                if 0 <= nx < 16 and 0 <= ny < 16 and src[nx, ny][3] > 100:
                    px[x, y] = OUT
                    break

def save(im, name):
    folder = os.path.join("Assets", "Art", "World")
    os.makedirs(folder, exist_ok=True)
    im.save(os.path.join(folder, name + ".png"))

def doctor():
    im, px = sprite()
    WHITE, SHADE, SKIN, RED, TEAL = (240, 240, 244), (190, 196, 208), (236, 190, 150), (214, 52, 60), (70, 160, 150)
    rect(px, 5, 1, 10, 2, WHITE)            # cap
    rect(px, 7, 1, 8, 2, RED); rect(px, 6, 2, 9, 2, RED)  # cross on the cap
    rect(px, 5, 3, 10, 6, SKIN)             # face
    rect(px, 6, 4, 6, 4, (40, 30, 40)); rect(px, 9, 4, 9, 4, (40, 30, 40))
    rect(px, 4, 7, 11, 14, WHITE)           # coat
    rect(px, 4, 12, 11, 14, SHADE)
    rect(px, 7, 7, 8, 11, TEAL)             # shirt under the coat
    rect(px, 10, 9, 12, 11, (120, 80, 50))  # medicine bag
    rect(px, 5, 15, 6, 15, (80, 56, 40)); rect(px, 9, 15, 10, 15, (80, 56, 40))
    outline(im, px)
    save(im, "doctor")

def crystal():
    im, px = sprite()
    STONE, STONE_D = (150, 150, 160), (100, 100, 116)
    rect(px, 3, 12, 12, 14, STONE); rect(px, 3, 14, 12, 14, STONE_D)
    rect(px, 5, 10, 10, 11, STONE_D)
    BODY, LIGHT, DARK = (90, 190, 255), (200, 240, 255), (50, 110, 200)
    rect(px, 6, 3, 9, 9, BODY)
    rect(px, 7, 1, 8, 2, BODY); rect(px, 5, 5, 5, 8, DARK); rect(px, 10, 5, 10, 8, DARK)
    rect(px, 6, 3, 6, 7, LIGHT); rect(px, 7, 1, 7, 2, LIGHT)
    rect(px, 9, 7, 9, 9, DARK)
    outline(im, px)
    save(im, "save_crystal")

def monument():
    im, px = sprite()
    STONE, STONE_L, STONE_D, GOLD = (156, 152, 160), (196, 192, 200), (104, 100, 116), (240, 196, 70)
    rect(px, 2, 13, 13, 14, STONE_D)       # base
    rect(px, 3, 5, 12, 12, STONE)          # slab
    rect(px, 3, 5, 12, 6, STONE_L)
    rect(px, 11, 7, 12, 12, STONE_D)
    rect(px, 7, 7, 8, 11, GOLD)            # golden star-ish mark
    rect(px, 5, 9, 10, 9, GOLD)
    rect(px, 4, 2, 11, 4, STONE_L); rect(px, 5, 1, 10, 1, STONE)   # top
    outline(im, px)
    save(im, "records_monument")

doctor(); crystal(); monument()
print("town art 2 written")
