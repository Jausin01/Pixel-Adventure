"""Generates the twin daggers, hammer and staff art plus the skill/attack effects.
Held sprites (pointing right) go to Assets/Art/Weapons/, item icons to Assets/Art/Items/, effects to Assets/Art/Effects/.
Run from the project root: python Tools/generate_weapon_art2.py"""
import math
from generate_weapon_art import METALS, Canvas, render

def dagger_layers(m, vo):
    # One dagger lying along u (0 = pommel, 20 = tip), shifted across by vo.
    def half(u):
        if u < 6 or u > 20:
            return -1
        return 1.5 * (20 - u) / 14.0 + 0.3 if u > 12 else 1.5
    blade = lambda u, v: abs(v - vo) <= half(u)
    edge = lambda u, v: blade(u, v) and v - vo > 0.2
    guard = lambda u, v: 5 <= u <= 6.4 and abs(v - vo) <= 2.5
    grip = lambda u, v: 1.5 <= u <= 5 and abs(v - vo) <= 0.9
    pommel = lambda u, v: 0 <= u <= 1.8 and abs(v - vo) <= 1.2
    return [(grip, m["shaft"]), (pommel, m["trim"]), (blade, m["body"]), (edge, m["light"]), (guard, m["trim"])]

def twin(m, spread):
    return dagger_layers(m, spread) + dagger_layers(m, -spread)

def hammer_layers(m):
    shaft = lambda u, v: 0 <= u <= 18 and abs(v) <= 1.0
    shine = lambda u, v: 1 <= u <= 17 and 0 <= v <= 0.8
    head = lambda u, v: 15 <= u <= 23.5 and abs(v) <= 6.2
    top = lambda u, v: head(u, v) and v > 4.4
    side = lambda u, v: head(u, v) and u > 21.3
    band = lambda u, v: 13.5 <= u <= 15 and abs(v) <= 1.8
    cap = lambda u, v: 0 <= u <= 1.8 and abs(v) <= 1.5
    return [(shaft, m["shaft"]), (shine, m["shaft_light"]), (cap, m["trim"]), (head, m["body"]), (side, m["dark"]), (top, m["light"]), (band, m["trim"])]

ORBS = {
    "oak":      {"orb": (88, 170, 240), "glow": (190, 232, 255), "dark": (44, 96, 170), "shaft": (120, 82, 50), "light": (156, 112, 70), "trim": (190, 150, 70)},
    "amethyst": {"orb": (168, 96, 226), "glow": (230, 190, 255), "dark": (96, 52, 150), "shaft": (84, 62, 92), "light": (120, 94, 130), "trim": (214, 214, 230)},
    "ruby":     {"orb": (236, 70, 66), "glow": (255, 190, 150), "dark": (140, 30, 44), "shaft": (70, 50, 54), "light": (104, 78, 80), "trim": (236, 190, 70)},
}

def staff_layers(o, orb_r=3.2, shaft_w=1.8):
    cu = 40.0
    shaft = lambda u, v: 0 <= u <= 36 and abs(v) <= shaft_w / 2
    shine = lambda u, v: 1 <= u <= 35 and 0 <= v <= shaft_w / 2 - 0.2
    claw = lambda u, v: 33 <= u <= 38.5 and orb_r * 0.55 <= abs(v) <= orb_r * 0.55 + 1.0 or (33 <= u <= 36 and abs(v) <= shaft_w / 2 + 1.2)
    orb = lambda u, v: (u - cu) ** 2 + v ** 2 <= orb_r ** 2
    dark = lambda u, v: orb(u, v) and (u - cu) ** 2 + v ** 2 > (orb_r - 1.1) ** 2 and v < 0.3
    glow = lambda u, v: (u - cu + 1) ** 2 + (v - 1) ** 2 <= (orb_r * 0.38) ** 2
    return [(shaft, o["shaft"]), (shine, o["light"]), (claw, o["trim"]), (orb, o["orb"]), (dark, o["dark"]), (glow, o["glow"])]

def blob(w, h, painter, folder, name):
    c = Canvas(w, h)
    for y in range(h):
        for x in range(w):
            col = painter(x + 0.5 - w / 2.0, y + 0.5 - h / 2.0)
            if col:
                c.px[x, y] = col
    c.save(folder, name)

def effects():
    blob(8, 8, lambda x, y: (255, 240, 150, 255) if abs(x) + abs(y) <= 3.2 or (abs(x) < 0.7 or abs(y) < 0.7) and abs(x) + abs(y) <= 4 else None, "Effects", "spark")
    blob(10, 10, lambda x, y: (176, 160, 140, 255) if x * x + y * y <= 4 else ((206, 192, 170, 200) if x * x + y * y <= 20 else None), "Effects", "dust")
    blob(16, 3, lambda x, y: (255, 255, 255, 255) if abs(y) < 0.6 else ((150, 210, 255, 255) if abs(y) < 1.6 else None), "Effects", "bolt")
    def fire(x, y):
        d = math.hypot(x, y)
        return (255, 244, 170, 255) if d <= 2 else ((255, 150, 40, 255) if d <= 4 else ((220, 70, 30, 255) if d <= 5.6 else None))
    blob(12, 12, fire, "Effects", "fireball")
    def boom(x, y):
        d = math.hypot(x, y); a = math.atan2(y, x)
        edge = 12 + 3.5 * math.sin(a * 7)
        if d > edge: return None
        return (255, 240, 170, 255) if d < 5 else ((255, 160, 50, 255) if d < 9 else (210, 70, 30, 255))
    blob(32, 32, boom, "Effects", "explosion")
    def magic(x, y):
        d = math.hypot(x, y)
        if d <= 2.2: return (236, 214, 255, 255)
        if d <= 3.2: return (168, 96, 226, 255)
        if x < -2 and abs(y) <= max(0, 2.8 + (x + 2) * 0.5): return (120, 70, 190, 220)
        return None
    blob(14, 8, magic, "Effects", "magic_bolt")
    def knife(x, y):
        if -4.5 <= x <= 2 and abs(y) <= max(0.3, 1.2 * (2 - x) / 6.5): return (214, 222, 232, 255)
        if -4.5 <= x <= -2.5 and abs(y) <= 0.8: return (110, 78, 50, 255)
        return None
    blob(10, 4, knife, "Effects", "knife")
    def ring(x, y):
        d = math.hypot(x, y)
        if d <= 15.0: return (255, 255, 255, 255) if d >= 13.4 else (255, 255, 255, 90)
        return None
    blob(32, 32, ring, "Effects", "warn_circle")
    blob(16, 16, lambda x, y: (255, 255, 255, 255) if abs(x) > 6.5 or abs(y) > 6.5 else (255, 255, 255, 90), "Effects", "warn_square")
    blob(8, 8, lambda x, y: ((150, 142, 130, 255) if x + y < 1 else (190, 182, 168, 255)) if math.hypot(x, y) <= 3.6 else None, "Effects", "rock")
    blob(10, 4, lambda x, y: (96, 190, 84, 255) if abs(y) <= max(0.3, 1.1 * (5 - x) / 9) and x > -5 else None, "Effects", "thorn")

def main():
    for name, m in METALS.items():
        render(twin(m, 3.4), 12, 8, 0.5, 4.0, 0.0, 0.55, 4).save("Weapons", "dagger_" + name)
        render(twin(m, 3.4), 16, 16, 2.0, 13.5, 45.0, 0.72, 3).save("Items", "daggers_" + name)
        render(hammer_layers(m), 15, 9, 0.5, 4.5, 0.0, 0.55, 4).save("Weapons", "hammer_" + name)
        render(hammer_layers(m), 16, 16, 1.5, 14.5, 45.0, 0.56, 3).save("Items", "hammer_" + name)
    for name, o in ORBS.items():
        render(staff_layers(o, 3.6, 2.6), 26, 6, 0.5, 3.0, 0.0, 0.55, 4).save("Weapons", "staff_" + name)
        render(staff_layers(o, 4.6, 3.2), 16, 16, 1.5, 14.5, 45.0, 0.36, 3).save("Items", "staff_" + name)
    effects()
    print("weapon art 2 written")

if __name__ == "__main__":
    main()
