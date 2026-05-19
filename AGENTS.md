# Repository Instructions

## Project Skills

This repository includes project-local Codex skills under `skills/`.

When the user asks to use `image-prompt-and-simplify`, or asks for an image prompt simplification/detail-level workflow, read and follow:

`skills/image-prompt-and-simplify/SKILL.md`

Use the exact `detail_level` provided by the user. If the user does not specify a level, use `detail_level: 2`.

For project-local use, do not require a global skill install. The skill should be used directly from this repository path.

If the user asks how to install this skill globally into their Codex skills home, refer to:

`docs/codex-skill-install.md`
