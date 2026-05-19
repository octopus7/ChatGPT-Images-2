---
name: image-detail-denoise
description: "Use when Codex needs to reduce tiny speckles, glitter-like dots, sparkly artifacts, noisy micro-shading, peppered highlights, grainy texture, or busy small light-and-shadow variations in an existing image while preserving composition, character identity, pose, and style. This skill requires imagegen / built-in image generation or image editing for the actual denoise result; do not replace the result with local filters, scripts, C#, Python, PIL, ImageMagick, ffmpeg, or other deterministic image processing."
---

# Image Detail Denoise Skill

## Skill Path

`skills/image-detail-denoise/SKILL.md`

## Mandatory Execution

Use `imagegen` for the actual image denoise/edit result.

Do not create the denoised image with local filters, blur, sharpen, resize, C#, Python, PIL, ImageMagick, ffmpeg, screenshots, or manual compositing. Local tools may only be used after imagegen output exists, for file copying, format conversion, resizing, README updates, or packaging.

If the target image is a local file path, load it with `view_image` first so it is visible in the conversation context, then invoke imagegen as an edit/generation task using that visible image as the edit target or reference.

If imagegen is unavailable or fails, stop and tell the user that this skill requires imagegen. Do not silently fall back to local image processing for the denoise result.

## Purpose

Use this skill when an image has appealing detail and texture, but contains excessive tiny speckles, glitter-like dots, noisy micro-shading, peppered highlights, or overly busy small light-and-shadow variations.

The goal is not to remove detail. The goal is to preserve the richness of the image while cleaning up visual noise.

## Core Principle

Preserve the original composition, character identity, pose, color palette, lighting direction, atmosphere, and important details.

Do not overly simplify the image unless the requested detail level is low.

Reduce only the noisy or distracting parts of the rendering, especially:

- tiny speckled dots
- random scattered highlights
- glitter-like noise
- peppered micro-texture
- grainy shading
- overly busy dappled light
- jagged micro-contrast
- pointillist-looking noise
- excessive small-value variations

Keep useful details such as:

- hair texture
- fabric texture
- wood grain
- stone texture
- water reflections
- skin softness
- foliage structure
- important props and accessories
- readable clothing folds
- material feel and depth

## Detail Level Control

Use `detail_level` to control how much detail should be preserved while reducing speckled noise.

### detail_level: 1 — Clean Simplified

Use this when the image is too visually busy and the user wants a noticeably cleaner result.

Preserve the main composition, character, pose, lighting direction, color palette, and atmosphere. Strongly reduce tiny speckles, glitter-like dots, noisy micro-texture, busy micro-shading, excessive small highlights, and overly detailed surface variation.

Organize light and shadow into broad, clean shapes. Keep only light texture and simplified detail. Hair, clothing folds, water reflections, foliage, rocks, and background elements should be readable but not densely rendered.

The result should look clean, calm, smooth, and simplified while still retaining the identity of the original image.

#### Prompt Template

Preserve the original composition, character, pose, lighting direction, color palette, and atmosphere. Simplify the rendering into a cleaner illustration. Strongly reduce tiny speckled dots, glitter-like highlights, noisy micro-textures, grainy shading, peppered highlights, and overly busy small-value variations. Organize light and shadow into broad, clean shapes. Keep only light texture and simplified detail. Make the final image clean, readable, smooth, and polished without changing the identity of the original image.

### detail_level: 2 — Balanced Detail

Use this as the default level.

Preserve the original detail, texture, atmosphere, and material feel. Reduce tiny speckles, noisy micro-contrast, grainy shading, and over-busy highlight patterns, but keep the image richly illustrated.

Light and shadow should be slightly broader and more organized, but not flat. Texture should remain visible in hair, fabric, wood, stone, water, skin, and foliage, but it should be cleaner and more controlled.

The result should look like a detailed high-quality illustration with less visual noise and smoother tonal transitions.

#### Prompt Template

Preserve the original composition, character, pose, lighting direction, color palette, atmosphere, and important details. Keep the rich depiction, material textures, and detailed illustration quality. Reduce tiny speckled dots, scattered glitter-like highlights, noisy micro-textures, grainy shading, peppered highlights, and overly busy small-value variations. Organize lighting and shadows into larger, cleaner shapes. Smooth out jagged micro-contrast while preserving texture in hair, fabric, wood, stone, water, skin, and foliage. The final result should look like a polished, detailed, high-quality illustration with preserved texture, smoother tonal transitions, less visual noise, fewer tiny dots, and clearer forms.

### detail_level: 3 — Rich Detail

Use this when the user likes the detailed look and only dislikes the tiny dot-like noise.

Preserve rich detail and material texture as much as possible. Keep hair strands, fabric folds, wood grain, stone texture, foliage detail, water detail, props, and small design elements. Do not flatten the image or remove meaningful texture.

Selectively reduce only random speckled dots, glitter noise, peppered highlights, and overly grainy micro-shading. Keep the image highly detailed, but make the light and shadow cleaner and less noisy.

The result should look like a rich, premium illustration with detailed texture, smoother tonal transitions, and no distracting pointillist speckling.

#### Prompt Template

Preserve the original composition, character, pose, lighting direction, color palette, atmosphere, and rich illustration detail. Keep hair strands, fabric folds, wood grain, stone texture, foliage detail, water reflections, props, accessories, and material texture. Do not overly simplify or flatten the image. Selectively reduce only random tiny speckles, glitter-like dots, peppered highlights, pointillist noise, and overly grainy micro-shading. Keep the detailed depiction intact, but make the light and shadow cleaner, smoother, and more controlled. The final image should remain richly detailed and premium-looking, without distracting dot-like noise.

## Korean Instructions

### 공통 지시사항

원본의 구도, 캐릭터, 포즈, 색감, 조명 방향, 분위기, 주요 디테일은 유지한다. 디테일을 무조건 줄이는 것이 아니라, 깨알 같은 점묘, 노이즈성 텍스처, 과도하게 잘게 쪼개진 명암만 정리한다.

### 1단계 — 정리형

원본의 구도와 분위기는 유지하되, 전체를 더 단순하고 깔끔하게 정리한다. 작은 점묘, 자잘한 하이라이트, 노이즈성 미세 텍스처, 자글자글한 명암 변화는 강하게 줄인다. 빛과 그림자는 큰 덩어리로 정리하고, 디테일은 선명하지만 간결하게 표현한다. 재질감은 남기되 전체적으로 정돈된 느낌을 우선한다.

### 2단계 — 균형형

원본의 디테일, 재질감, 분위기, 색감은 유지한다. 다만 화면에 흩어지는 깨알 같은 점, 자잘한 반짝임, 과도한 미세 명암 변화, 노이즈성 텍스처만 줄인다. 빛과 그림자는 너무 잘게 쪼개지지 않도록 조금 더 큰 면으로 정리하고, 텍스처는 깨끗하고 통제된 방식으로 유지한다. 결과물은 디테일이 살아 있으면서도 훨씬 정돈된 고품질 일러스트처럼 보이게 한다.

### 3단계 — 풍부형

원본의 풍부한 묘사와 텍스처를 최대한 유지한다. 머리카락, 옷 주름, 나무결, 돌 표면, 물결, 식생의 세부 묘사는 살리되, 화면 전체를 지저분하게 보이게 하는 깨알 같은 점, 랜덤한 반짝임, 노이즈성 미세 입자, 과도하게 자글거리는 명암만 선택적으로 줄인다. 디테일은 풍부하게 유지하되, 명암 전환은 더 매끄럽고 통제된 방식으로 정리한다. 결과물은 디테일이 많은 고급 일러스트이되, 점묘성 노이즈 없이 정리된 상태여야 한다.

## Negative Prompt

Use these terms when the image model supports negative prompts:

tiny speckles, scattered dots, glitter noise, grainy texture, noisy shading, peppered highlights, pointillist texture, excessive micro-detail, harsh micro-contrast, jagged shadows, busy dappled light, over-sharpened details, random white dots, random black dots, dirty texture, visual noise, rough stippling, cluttered highlights, noisy micro-texture, granular highlights, speckled lighting

## Do

- Preserve the identity of the original image.
- Keep meaningful texture and material feeling.
- Keep detailed illustration quality when requested.
- Reduce visual noise and dot-like artifacts.
- Use smoother tonal transitions.
- Make forms easier to read.
- Keep lighting direction and mood consistent.
- Choose the detail level based on the user's stated preference.

## Do Not

- Do not flatten the image too much unless `detail_level: 1` is requested.
- Do not remove all texture.
- Do not blur the image to hide noise.
- Do not change the composition unless requested.
- Do not change character identity.
- Do not remove important props or accessories.
- Do not make the result look plastic, airbrushed, or low-detail.
- Do not confuse useful texture with unwanted speckled noise.

## Recommended Defaults

If the user does not specify a level, use:

`detail_level: 2 — Balanced Detail`

If the user says the first result is too simple, move upward to:

`detail_level: 3 — Rich Detail`

If the user says the image is still too busy, noisy, or cluttered, move downward to:

`detail_level: 1 — Clean Simplified`
