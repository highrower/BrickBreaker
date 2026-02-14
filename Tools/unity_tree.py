#!/usr/bin/env python3
from __future__ import annotations

import os
from pathlib import Path

# Folders Unity projects usually shouldn't share as "structure"
DEFAULT_EXCLUDE_DIRS = {
    ".git", ".idea", ".vs",
    "Library", "Temp", "Obj", "Logs",
    "Build", "Builds",
    "UserSettings", "Unity"
}

DEFAULT_EXCLUDE_EXTS = {".meta"}

def should_skip_dir(name: str, exclude_dirs: set[str]) -> bool:
    return name in exclude_dirs

def should_skip_file(path: Path, exclude_exts: set[str]) -> bool:
    return path.suffix in exclude_exts

def print_tree(root: Path, max_depth: int | None = None,
               exclude_dirs: set[str] | None = None,
               exclude_exts: set[str] | None = None) -> None:
    exclude_dirs = exclude_dirs or set()
    exclude_exts = exclude_exts or set()

    root = root.resolve()
    print(f"{root.name}/")

    def walk(dir_path: Path, prefix: str, depth: int) -> None:
        if max_depth is not None and depth > max_depth:
            return

        try:
            entries = sorted(dir_path.iterdir(), key=lambda p: (p.is_file(), p.name.lower()))
        except PermissionError:
            return

        # Filter
        filtered: list[Path] = []
        for p in entries:
            if p.is_dir():
                if not should_skip_dir(p.name, exclude_dirs):
                    filtered.append(p)
            else:
                if not should_skip_file(p, exclude_exts):
                    filtered.append(p)

        for i, p in enumerate(filtered):
            last = (i == len(filtered) - 1)
            branch = "└── " if last else "├── "
            next_prefix = prefix + ("    " if last else "│   ")

            if p.is_dir():
                print(f"{prefix}{branch}{p.name}/")
                walk(p, next_prefix, depth + 1)
            else:
                print(f"{prefix}{branch}{p.name}")

    walk(root, "", 1)

if __name__ == "__main__":
    # Change this to "Assets" if you only care about that subtree
    start = Path(".")

    # Common use: show only the meaningful Unity stuff
    # e.g. root = Path("Assets")
    root = Path("Assets") if Path("Assets").exists() else start

    print_tree(
        root=root,
        max_depth=None,  # set to e.g. 4 if it's huge
        exclude_dirs=DEFAULT_EXCLUDE_DIRS,
        exclude_exts=DEFAULT_EXCLUDE_EXTS,
    )
