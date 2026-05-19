# GPT-Image2

<p align="center">
  <a href="https://youtu.be/CdE4gloKKsw">
    <img src="Video/youtube-thumbnail-2stage-original-denoised-480p-play.jpg" alt="Watch ChatGPT Images 2.0: Sparkly Artifacts Fixed on YouTube" width="854">
  </a>
</p>

## Image Detail Denoise Results

<table>
  <tr>
    <td width="50%" align="center" valign="top">
      <img src="Images/ed_splash_denoise_detail_1_clean_simplified.jpg" alt="Detail Level 1 - Clean Simplified" width="100%">
      <br>
      <strong>Detail Level 1 - Clean Simplified</strong><br>
      Strongly reduces speckled noise and busy micro-texture while keeping the original composition readable.<br>
      점 형태의 노이즈와 복잡한 미세 질감을 강하게 줄이면서 원본 구도를 읽기 쉽게 유지합니다.<br>
      点状のノイズと複雑な微細テクスチャを大きく抑えながら、元の構図を読み取りやすく保ちます。
    </td>
    <td width="50%" align="center" valign="top">
      <img src="Images/ed_splash_denoise_detail_2_balanced.jpg" alt="Detail Level 2 - Balanced Detail" width="100%">
      <br>
      <strong>Detail Level 2 - Balanced Detail</strong><br>
      Preserves rich illustration detail while smoothing noisy micro-contrast and scattered highlights.<br>
      풍부한 일러스트 디테일을 유지하면서 노이즈성 미세 대비와 흩어진 하이라이트를 부드럽게 정리합니다.<br>
      豊かなイラストのディテールを保ちながら、ノイズのある微細なコントラストと散ったハイライトを滑らかに整えます。
    </td>
  </tr>
  <tr>
    <td width="50%" align="center" valign="top">
      <img src="Images/ed_splash_denoise_detail_3_rich.jpg" alt="Detail Level 3 - Rich Detail" width="100%">
      <br>
      <strong>Detail Level 3 - Rich Detail</strong><br>
      Keeps the detailed rendering intact and selectively removes distracting dot-like noise.<br>
      세밀한 렌더링은 그대로 유지하고 방해되는 점 형태의 노이즈만 선택적으로 제거합니다.<br>
      細密なレンダリングはそのまま保ち、邪魔になる点状ノイズだけを選択的に取り除きます。
    </td>
    <td width="50%" align="center" valign="top">
      <img src="Images/EdSplash.jpg" alt="Original Image" width="100%">
      <br>
      <strong>Original Image</strong><br>
      Unprocessed source image used as the comparison baseline.<br>
      비교 기준으로 사용하는 미처리 원본 이미지입니다.<br>
      比較基準として使用する未処理の元画像です。
    </td>
  </tr>
</table>

## Codex Skills Usage

This repository includes a Codex-only skill:<br>
이 저장소에는 Codex 전용 스킬이 포함되어 있습니다.<br>
このリポジトリには Codex 専用スキルが含まれています。

`skills/image-detail-denoise/SKILL.md`

Use this skill in Codex when an image has appealing detail and texture but also contains excessive tiny speckles, glitter-like dots, noisy micro-shading, peppered highlights, or overly busy small light-and-shadow variations.<br>
이미지에 매력적인 디테일과 질감이 있지만 작은 반점, 반짝이는 점, 노이즈성 미세 음영, 흩뿌려진 하이라이트, 지나치게 복잡한 명암 변화가 많을 때 Codex에서 이 스킬을 사용합니다.<br>
画像に魅力的なディテールと質感がありつつ、小さな斑点、きらめく点、ノイズのある微細な陰影、散ったハイライト、過度に複雑な明暗変化が多い場合に Codex でこのスキルを使用します。

Example Codex request:

```text
Use the image-detail-denoise skill on this image with detail_level: 2.
```

Available detail levels:

- `detail_level: 1` - Clean Simplified
- `detail_level: 2` - Balanced Detail
- `detail_level: 3` - Rich Detail

This skill is designed for Codex only. It is not compatible with other agents.<br>
이 스킬은 Codex 전용으로 설계되었으며 다른 에이전트에서는 사용할 수 없습니다.<br>
このスキルは Codex 専用に設計されており、他のエージェントでは使用できません。
