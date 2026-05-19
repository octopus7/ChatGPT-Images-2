# Codex Skill Install

This repository includes the project-local Codex skill:

`skills/image-prompt-and-simplify/SKILL.md`

Use project-local setup when you only need the skill inside this repository. Use global install when you want the skill available from new Codex chats in other repositories too.

## Project-Local Use

No install step is required for project-local use.

This repository's `AGENTS.md` tells Codex to read and follow:

`skills/image-prompt-and-simplify/SKILL.md`

From this repository, you can ask Codex:

```text
Use the image-prompt-and-simplify skill on this image with detail_level: 2.
```

If `detail_level` is omitted, the project default is `detail_level: 2`.

## Global Install

To make the skill available from any repository, copy the skill folder into your Codex skills home.

### Windows PowerShell

If `CODEX_HOME` is set:

```powershell
New-Item -ItemType Directory -Force -Path "$env:CODEX_HOME\skills" | Out-Null
Copy-Item -Recurse -Force `
  ".\skills\image-prompt-and-simplify" `
  "$env:CODEX_HOME\skills\image-prompt-and-simplify"
```

If `CODEX_HOME` is not set, use the default user Codex home:

```powershell
New-Item -ItemType Directory -Force -Path "$env:USERPROFILE\.codex\skills" | Out-Null
Copy-Item -Recurse -Force `
  ".\skills\image-prompt-and-simplify" `
  "$env:USERPROFILE\.codex\skills\image-prompt-and-simplify"
```

### macOS/Linux

If `CODEX_HOME` is set:

```bash
mkdir -p "$CODEX_HOME/skills"
cp -R skills/image-prompt-and-simplify "$CODEX_HOME/skills/"
```

If `CODEX_HOME` is not set, use the default user Codex home:

```bash
mkdir -p ~/.codex/skills
cp -R skills/image-prompt-and-simplify ~/.codex/skills/
```

## Verify

Start a new Codex chat and check that `image-prompt-and-simplify` appears in the available skills list. If it does not appear, confirm that the installed path contains:

`image-prompt-and-simplify/SKILL.md`

For project-local use, the skill may not appear in the global available-skills list. In this repository, `AGENTS.md` still instructs Codex to use the local skill path directly.
