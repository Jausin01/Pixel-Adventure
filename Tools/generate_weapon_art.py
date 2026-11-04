"""Generates the axe, spear, shuriken and boomerang art.
Held sprites (pointing right) go to Assets/Art/Weapons/, item icons to Assets/Art/Items/.
Run from the project root: python Tools/generate_weapon_art.py"""
import math, os
from PIL import Image

ART = os.path.join("Assets", "Art")
OUT = (28, 22, 34)

METALS = {
    "iron":  {"body": (150, 156, 166), "light": (206, 212, 220), "dark": (92, 98, 112), "shaft": (120, 82, 50), "shaft_light": (156, 112, 70), "trim": (190, 150, 70)},
    "steel": {"body": (128, 158, 198), "light": (196, 220, 244), "dark": (80, 104, 144), "shaft": (96, 70, 56), "shaft_light": (132, 100, 78), "trim": (210, 214, 224)},
    "ruby":  {"body": (196, 56, 66), "light": (246, 128, 112), "dark": (122, 28, 46), "shaft": (70, 50, 54), "shaft_light": (104, 78, 80), "trim": (236, 190, 70)},
}


class Canvas:
    def __init__(self, w, h):
        self.w, self.h = w, h
        self.im = Image.new("RGBA", (w, h), (0, 0, 0, 0))
        self.px = self.im.load()

    def outline(self):
        src = self.im.copy().load()
        for y in range(self.h):
            for x in range(self.w):
                if src[x, y][3] > 100:
                    continue
                for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
                    nx, ny = x + dx, y + dy
                    if 0 <= nx < self.w and 0 <= ny < self.h and src[nx, ny][3] > 100:
                        self.px[x, y] = OUT + (255,)
                        break

    def save(self, folder, name):
        path = os.path.join(ART, folder)
        os.makedirs(path, exist_ok=True)
        self.im.save(os.path.join(path, name + ".png"))


def render(layers, w, h, cx, cy, angle=0.0, scale=1.0, supersample=1):
    """Paints layers (function(u, v) -> bool, color) given in weapon space (u along the weapon, v across, v up),
    placed with its origin at pixel (cx, cy), rotated by angle degrees (counter-clockwise) and scaled."""
    canvas = Canvas(w, h)
    ca, sa = math.cos(math.radians(angle)), math.sin(math.radians(angle))
    n = supersample
    for py in range(h):
        for px in range(w):
            votes = {}
            for sy in range(n):
                for sx in range(n):
                    x = px + (sx + 0.5) / n - cx
                    y = -(py + (sy + 0.5) / n - cy)  # image y goes down, v goes up
                    u = (x * ca + y * sa) / scale
                    v = (-x * sa + y * ca) / scale
                    color = None
                    for test, col in layers:
                        if test(u, v):
                            color = col
                    if color is not None:
                        votes[color] = votes.get(color, 0) + 1
            if votes and sum(votes.values()) * 2 > n * n:
                canvas.px[px, py] = max(votes, key=votes.get) + (255,)
    canvas.outline()
    return canvas


def circle(cu, r):
    return lambda u, v: (u - cu) ** 2 + v ** 2 <= r * r


def axe_layers(m, shaft_width):
    # Weapon space: u from 0 (butt) to 24 (blade edge), v across. Head is a crescent opening toward the handle.
    c1, r1, c2, r2 = 16.0, 7.6, 9.8, 6.4
    inside = lambda u, v: (u - c1) ** 2 + v ** 2 <= r1 * r1 and (u - c2) ** 2 + v ** 2 > r2 * r2
    rim = lambda u, v: inside(u, v) and (u - c1) ** 2 + v ** 2 > (r1 - 1.3) ** 2
    inner = lambda u, v: inside(u, v) and (u - c2) ** 2 + v ** 2 <= (r2 + 1.2) ** 2
    shaft = lambda u, v: 0 <= u <= 17 and abs(v) <= shaft_width / 2
    shine = lambda u, v: 1.5 <= u <= 17 and 0 <= v <= shaft_width / 2 - 0.2
    cap = lambda u, v: 0 <= u <= 2 and abs(v) <= shaft_width / 2 + 0.5
    return [(shaft, m["shaft"]), (shine, m["shaft_light"]), (cap, m["trim"]), (inside, m["body"]), (inner, m["dark"]), (rim, m["light"])]


def spear_layers(m, shaft_width, head_scale=1.0):
    # Weapon space: u from 0 (butt) to 40 (tip).
    def half(u):
        if u < 29 or u > 39.5:
            return -1
        if u <= 32:
            return (1.0 + (u - 29) * 0.6) * head_scale
        return 2.8 * head_scale * (39.5 - u) / 7.5
    head = lambda u, v: abs(v) <= half(u)
    edge = lambda u, v: head(u, v) and v > 0 and abs(v) >= half(u) - 1.0
    shaft = lambda u, v: 0 <= u <= 30 and abs(v) <= shaft_width / 2
    shine = lambda u, v: 1 <= u <= 29 and 0 <= v <= shaft_width / 2 - 0.2
    collar = lambda u, v: 28 <= u <= 30 and abs(v) <= shaft_width / 2 + 0.9
    tassel = lambda u, v: 24.5 <= u <= 28.5 and -3.4 <= v <= -shaft_width / 2
    return [(shaft, m["shaft"]), (shine, m["shaft_light"]), (tassel, (190, 48, 52)), (head, m["body"]), (edge, m["light"]), (collar, m["trim"])]


def shuriken_layers(m):
    blade = lambda u, v: abs(u) ** 0.6 + abs(v) ** 0.6 <= 6.9 ** 0.6 and u * u + v * v > 1.5 ** 2
    shine = lambda u, v: blade(u, v) and (u > 0.3 and v > 0.3 or u < -0.3 and v < -0.3)
    hub = lambda u, v: 1.5 ** 2 < u * u + v * v <= 2.6 ** 2
    return [(blade, m["body"]), (shine, m["light"]), (hub, m["dark"])]


def boomerang_layers(body, light, dark):
    def polar(u, v):
        return math.hypot(u, v), math.degrees(math.atan2(v, u))
    def width(a):
        t = max(0.0, min(1.0, (a - 60) / 135.0))
        return 1.0 + 1.0 * math.sin(math.pi * t)
    def arc(u, v):
        r, a = polar(u, v)
        return 60 <= a <= 195 and abs(r - 5.6) <= width(a)
    edge = lambda u, v: arc(u, v) and polar(u, v)[0] > 5.6 + width(polar(u, v)[1]) - 0.8
    core = lambda u, v: arc(u, v) and polar(u, v)[0] < 5.6 - width(polar(u, v)[1]) + 0.8
    return [(arc, body), (core, dark), (edge, light)]


def shuriken_silhouette():
    canvas = Canvas(16, 16)
    for y in range(16):
        for x in range(16):
            u, v = x + 0.5 - 8, 8 - (y + 0.5)
            if abs(u) ** 0.6 + abs(v) ** 0.6 <= 6.4 ** 0.6 and u * u + v * v > 1.2 ** 2:
                canvas.px[x, y] = (35, 25, 20, 150)
    canvas.save("Hud", "slot_secondary")


def main():
    for name, m in METALS.items():
        render(axe_layers(m, 2.0), 15, 11, 1.0, 5.5, 0.0, 0.55, 3).save("Weapons", "axe_" + name)
        render(spear_layers(m, 2.6), 24, 6, 1.0, 3.0, 0.0, 0.55, 3).save("Weapons", "spear_" + name)
        render(spear_layers(m, 4.0, 1.7), 16, 16, 1.5, 14.5, 45.0, 0.38, 3).save("Items", "spear_" + name)
    for name in ("iron", "steel"):
        render(shuriken_layers(METALS[name]), 16, 16, 8.0, 8.0, 0.0, 1.0, 3).save("Items", "shuriken_" + name)
    render(boomerang_layers((176, 124, 72), (212, 164, 100), (116, 76, 44)), 16, 16, 10.5, 10.5, 0.0, 1.0, 3).save("Items", "boomerang_wood")
    render(boomerang_layers((232, 224, 200), (252, 248, 232), (172, 160, 132)), 16, 16, 10.5, 10.5, 0.0, 1.0, 3).save("Items", "boomerang_bone")
    shuriken_silhouette()
    print("weapon art written to", ART)


if __name__ == "__main__":
    main()
