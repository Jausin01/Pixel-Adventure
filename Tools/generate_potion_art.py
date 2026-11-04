"""Pixel art for the health/energy potions and the menu button. Run from the project root: python3 Tools/generate_potion_art.py"""
import os
from PIL import Image

OUT = os.path.join("Assets", "Art")

def potion(name, liquid, dark, light):
    img = Image.new("RGBA", (16, 16), (0, 0, 0, 0))
    shape = {}
    for y in range(16):
        for x in range(16):
            dx, dy = (x - 7.5) / 5.2, (y - 10.2) / 4.8
            if dx * dx + dy * dy <= 1.0 and y >= 6:
                shape[(x, y)] = "body"
    for y in range(4, 7):
        for x in range(6, 10):
            shape[(x, y)] = "neck"
    for y in range(2, 4):
        for x in range(6, 10):
            shape[(x, y)] = "cork"
    def inside(p): return p in shape
    for (x, y), part in shape.items():
        edge = any(not inside((x + a, y + b)) for a, b in ((1, 0), (-1, 0), (0, 1), (0, -1)))
        if edge:
            img.putpixel((x, y), (0, 0, 0, 255)); continue
        if part == "cork": c = (150, 98, 54, 255)
        elif part == "neck": c = (205, 232, 240, 255)
        else:
            if y < 9: c = (205, 232, 240, 255)       # empty glass above the liquid
            elif x > 9 or y > 12: c = dark
            else: c = liquid
        img.putpixel((x, y), c)
    img.putpixel((5, 10), light); img.putpixel((5, 11), light); img.putpixel((6, 9), (255, 255, 255, 255))
    img.save(os.path.join(OUT, "Items", name + ".png"))

def menu_icon():
    img = Image.new("RGBA", (16, 16), (0, 0, 0, 0))
    c = (120, 144, 156, 255)
    for y in (4, 5, 7, 8, 10, 11):
        for x in range(2, 14):
            img.putpixel((x, y), c)
    img.save(os.path.join(OUT, "Hud", "menu.png"))

if __name__ == "__main__":
    potion("health_potion", (229, 28, 35, 255), (150, 14, 28, 255), (255, 150, 150, 255))
    potion("energy_potion", (50, 140, 255, 255), (28, 84, 190, 255), (160, 210, 255, 255))
    menu_icon()
    print("done")
