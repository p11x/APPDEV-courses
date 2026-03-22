# Example300: Copy Module
import copy

print("Copy Module:")
# Shallow copy
original = [1, [2, 3], 4]
shallow = copy.copy(original)
shallow[1].append(5)
print(f"Original after shallow: {original}")
print(f"Shallow: {shallow}")

# Deep copy
original2 = [1, [2, 3], 4]
deep = copy.deepcopy(original2)
deep[1].append(5)
print(f"\nOriginal after deep: {original2}")
print(f"Deep: {deep}")

# Copy with dict
d = {'a': [1, 2], 'b': [3, 4]}
d_copy = d.copy()
d_copy['a'].append(3)
print(f"\nDict original: {d}")
print(f"Dict copy: {d_copy}")

# deepcopy dict
d_deep = copy.deepcopy(d)
d_deep['a'].append(4)
print(f"\nDeepcopy original: {d}")
print(f"Deepcopy: {d_deep}")
