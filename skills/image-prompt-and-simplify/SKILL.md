# Image Prompt and Simplify

## Purpose

Use this skill when the user provides an image and wants it processed into a cleaner, simpler, or detail-controlled version without losing the original scene identity.

This skill is intentionally 2-step:

1. Write a prompt that matches the uploaded image.
2. Apply that prompt with image_gen using a selected detail level.

The goal is not blind simplification. The goal is to preserve the original image's composition, subject identity, pose, layout, atmosphere, lighting, scene density, world detail, material feel, and texture, while controlling noisy tiny speckles, glitter-like dot noise, over-busy micro-shading, jagged small-value contrast, and excessive visual busyness.

---

## When to Use

Use this skill when the user says things like:

- “이 이미지를 심플하게 처리해줘”
- “이 이미지에 맞는 프롬프트 만들고 적용해줘”
- “디테일은 유지하되 더 깔끔하게 정리해줘”
- “깨알 같은 점은 줄이고 싶어”
- “이미지 기반으로 프롬프트를 작성해서 다시 생성해줘”
- “원래 구도 유지하면서 간결하게 다듬어줘”
- “레벨 1로 정리해줘”
- “레벨 2로 균형 있게”
- “레벨 3로 묘사는 살리고 노이즈만 줄여줘”

---

## Core Principle

Always treat the task as a reference-based image transformation.

The assistant must first understand what is in the image, then write a prompt that captures the image faithfully, and finally use that prompt to transform the image with image_gen.

Do not skip directly to generic simplification language without grounding the prompt in the uploaded image.

---

## Detail Level Control

Use detail_level to control how much detail should be preserved while reducing speckled noise and over-busy micro-shading.

If the user does not specify a level, use detail_level: 2 as the default.

### detail_level: 1 — Clean Simplified

Use this when the user wants the image clearly simplified, cleaner, and easier to read.

Preserve the main composition, subject identity, pose, lighting, color palette, and core atmosphere, but strongly reduce tiny speckles, glitter-like dots, noisy micro-texture, excessive small folds, busy micro-shading, and random scattered highlights.

Organize light and shadow into broad, clean shapes. Keep only light texture and simplified detail. The result should look cleaner and more graphic, but not empty, blurry, or plastic.

Prompt clause:

Apply detail_level 1: Clean Simplified. Preserve the original composition and subject identity, but strongly reduce tiny speckles, glitter-like dots, noisy micro-texture, excessive fine folds, and busy micro-shading. Organize light and shadow into broad, clean shapes. Keep only light texture and simplified detail while maintaining a polished illustration look.

---

### detail_level: 2 — Balanced Detail

Use this as the default. It balances detail preservation with cleaner rendering.

Preserve the original detail, texture, atmosphere, material feel, composition, and scene density, but reduce tiny speckles, noisy micro-contrast, over-busy shading, and random dot-like visual artifacts.

Keep texture visible, but cleaner and more controlled. Light and shadow should be slightly broader and easier to read, without removing the appeal of the original image.

Prompt clause:

Apply detail_level 2: Balanced Detail. Preserve the original detail, texture, atmosphere, material feel, composition, and scene density, but reduce tiny speckles, noisy micro-contrast, over-busy shading, and random dot-like artifacts. Keep texture visible, cleaner, and more controlled, with smoother tonal transitions and clear readable forms.

---

### detail_level: 3 — Rich Detail

Use this when the user likes the original richness and only wants the unwanted tiny dot noise reduced.

Preserve rich detail and material texture as much as possible. Keep hair strands, fabric folds, wood grain, stone texture, foliage detail, water detail, background density, and environmental complexity.

Selectively reduce only random speckled dots, glitter noise, overly grainy micro-shading, peppered highlights, and jagged micro-contrast. Maintain a highly detailed image with smoother tonal transitions and less visual noise.

Prompt clause:

Apply detail_level 3: Rich Detail. Preserve rich detail and material texture as much as possible, including hair strands, fabric folds, wood grain, stone texture, foliage detail, water detail, background density, and environmental complexity. Selectively reduce only random speckled dots, glitter noise, overly grainy micro-shading, peppered highlights, and jagged micro-contrast. Maintain a highly detailed polished illustration with smoother tonal transitions and less visual noise.

---

## Detail Level Selection Rule

Choose the level based on the user's wording:

- If the user says simple, cleaner, simplified, flat, less detail, 정리, 심플, use detail_level: 1.
- If the user says balanced, natural, keep detail but clean it, 적당히, 균형, use detail_level: 2.
- If the user says keep the original density, rich detail, 묘사는 살려, 텍스쳐 유지, 깨알만 제거, 원래 밀도 유지, use detail_level: 3.

When in doubt, use detail_level: 2.

---

## Two-Step Workflow

### Step 1 — Build an Image-Matched Prompt

Inspect the image and write a prompt that captures:

- main subject
- important props
- environment
- composition / camera distance / framing
- lighting and mood
- color palette
- style cues
- texture density
- the selected detail level
- the intended simplification or cleanup direction

The prompt must:

- clearly describe the uploaded image
- preserve the original scene structure
- preserve scene density unless the user explicitly asks to reduce it
- preserve wide composition if the original image is wide
- preserve background information if it is part of the image's appeal
- include the selected detail level instruction
- reduce noise without damaging identity or composition

Prompt structure:

1. Scene summary
2. Subject description
3. Environment description
4. Composition / framing description
5. Lighting / color / mood description
6. Detail level instruction
7. Preservation instruction
8. Anti-drift instruction

Example:

A wide scenic anime illustration of a silver-haired maid girl sitting on a rustic wooden bridge over a crystal-clear forest stream, barefoot with her feet in the water, holding a small food can and spoon. Her black shoes and white socks are placed beside her. The scene is surrounded by lush green trees, mossy rocks, shallow transparent water with visible pebbles, and small distant cascades. Warm sunlight filters through the leaves, creating a peaceful summer atmosphere. Apply detail_level 3: preserve the original wide composition, environmental density, detailed natural textures, and calm mood. Keep the textures readable and rich, but selectively reduce tiny speckles, glitter-like dot noise, and overly busy micro-shading. Maintain smoother tonal transitions and clearer forms without zooming in or reducing the scene density.

---

### Step 2 — Apply the Prompt with image_gen

After creating the prompt, call image_gen using the uploaded image as the reference image.

Requirements:

- Pass the uploaded image in reference_image_paths.
- Treat the uploaded image as Image A.
- Tell image_gen that Image A is the scene reference and that the result should preserve its composition and identity.
- Include the selected detail_level wording directly in the prompt.
- If the task is to transform the image rather than replace it, write an edit-style prompt.
- Preserve framing and scene density unless the user explicitly asks otherwise.
- If the original image is wide, keep a wide aspect ratio.

Prompt pattern:

Edit Image A into a cleaner, detail-controlled version while preserving the original composition, scene density, character identity, pose, environment, and lighting. Keep the wide scenic framing and the detailed forest stream setting. Apply detail_level [1/2/3]: [insert level-specific clause]. Keep the textures in the water, rocks, wood, foliage, hair, and clothing, but render them in a cleaner, more controlled way with smoother tonal transitions and clearer forms. Do not zoom in or narrow the field of view. Do not remove important background elements.

---

## Default Behavior

Unless the user says otherwise, use:

detail_level: 2

Default transformation means:

- keep detail
- keep texture
- keep scene density
- keep atmosphere
- reduce visual noise only
- avoid unwanted cropping or zooming

This is especially important for rich background art, environmental illustrations, anime scenery, and images where the appeal depends on a dense world and wide framing.

---

## Scene Density Rule

If the original image is visually dense, do not accidentally thin it out.

Preserve:

- number of visible background elements
- environmental richness
- foliage and rock presence
- wide view / spatial depth
- layered background structures

Simplification should affect rendering quality, not content quantity.

Good:

- cleaner foliage rendering
- smoother shading
- less speckled rocks
- less noisy water sparkle

Bad:

- zooming in too much
- cropping tighter than the original
- removing major background features
- flattening the environment into empty space
- over-simplifying the character and world

---

## Reusable Prompt Clauses

### Preservation Clauses

- Preserve the original composition and framing.
- Preserve the original scene density and environmental richness.
- Preserve the character identity, pose, and outfit.
- Preserve the lighting direction, atmosphere, and color palette.
- Preserve detailed textures, but make them cleaner and more controlled.
- Keep the wide scenic view and spatial depth.

### Cleanup Clauses

- Reduce tiny speckled dots and glitter-like noise.
- Reduce noisy micro-textures and grainy micro-shading.
- Organize light and shadow into cleaner, more readable shapes.
- Smooth out jagged micro-contrast.
- Keep detail, but remove random dot-like visual noise.
- Keep texture richness while reducing clutter.

### Anti-Drift Clauses

- Do not zoom in.
- Do not narrow the field of view.
- Do not reduce scene density.
- Do not remove important background elements.
- Do not make the result empty or overly simplified.
- Do not make the result blurry or plastic.

---

## Output Format for the Assistant

When using this skill, the assistant should normally produce:

1. a concise image-matched prompt draft
2. a selected detail_level
3. an image_gen call that applies the prompt
4. the final generated image result

If the user asks for the prompt first, provide the prompt before generating.
If the user asks to apply it immediately, generate directly after drafting the prompt internally.

---

## Compact Working Template

Analyze the uploaded image first. Write a prompt that accurately describes the subject, environment, composition, lighting, mood, and texture density. Select detail_level 1, 2, or 3 based on the user's requested amount of detail. Then apply that prompt with image_gen using the uploaded image as Image A. Preserve composition, scene density, subject identity, and atmosphere. Adjust only the rendering by reducing tiny speckles, dot noise, glitter-like highlights, noisy micro-texture, and over-busy micro-shading according to the selected detail level. Keep textures, but make them cleaner, smoother, and more controlled. Do not zoom in, crop tighter, or remove major background details unless explicitly requested.

---

## Example image_gen Delegation Template

Prompt:

Edit Image A into a cleaner, detail-controlled version while preserving the original composition, wide framing, scene density, character identity, environment, and lighting. Image A shows [brief grounded image summary]. Apply detail_level [1/2/3]: [insert level-specific clause]. Keep the textures in the foliage, rocks, wood, water, hair, and clothing, but reduce unwanted speckled visual noise according to the selected level. Organize light and shadow into smoother, clearer forms. Do not zoom in or reduce background density.

reference_image_paths:

- <uploaded image path>

---

## Do

- Ground the prompt in the uploaded image.
- Preserve the original image identity.
- Preserve composition and spatial layout.
- Select and state the correct detail level.
- Preserve scene density unless asked not to.
- Use simplification as controlled cleanup, not content removal.
- Keep detail and texture readable.

## Do Not

- Do not use a generic prompt that ignores the uploaded image.
- Do not simplify so much that the image loses its appeal.
- Do not crop tighter unless the user asks.
- Do not replace the environment with a less detailed one.
- Do not confuse cleanup with flattening.
- Do not remove important props or secondary structures.
- Do not treat detail_level 3 as simplification of content; it is noise cleanup while preserving richness.
