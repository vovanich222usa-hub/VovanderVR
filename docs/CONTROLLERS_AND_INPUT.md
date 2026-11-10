# Meta Quest 3 controllers and input

## Required in-game controller appearance

Use Meta's official runtime controller models, not generic hand meshes.

Recommended OpenXR route:
1. Add Meta's `OVRCameraRig` from Meta XR Core SDK.
2. Under `OVRCameraRig > TrackingSpace > LeftHandAnchor > LeftControllerAnchor`, add `OVRRuntimeControllerPrefab` and set Controller = `L Touch`.
3. Under `RightHandAnchor > RightControllerAnchor`, add the same prefab and set Controller = `R Touch`.
4. In OVR Manager > Quest Features > General, set Render Model Support = Enabled.
5. Enable the prefab's controller animation support. Meta's `OVRRuntimeController` supports animated trigger, buttons and thumbstick states from live input.

This loads the correct model from the Quest runtime, matching the actual controller generation. It avoids shipping a hand-drawn replacement model. It also means the exact model asset is provided by the runtime / Meta SDK, not copied into this repository.

Official Meta documentation:
- Runtime controller models: https://developers.meta.com/vr/documentation/unity/unity-runtime-controller/
- OVRRuntimeController animation support: https://developers.meta.com/vr/reference/unity/v76/class_o_v_r_runtime_controller/
- Controller input best practices: https://developers.meta.com/vr/documentation/unity/unity-controllers-best-practices/

## Agreed default controls

| Input | Default action |
|---|---|
| Left stick left/right | Smooth turn left/right (yaw) |
| Left stick up/down | Move forward/backward |
| Right controller B | Show/hide in-world Yandex map |
| Right trigger | Select / confirm pointed item |
| Left/right controller grip | Reserved for contextual interaction |
| Right stick | Optional panorama-link navigation; configurable |
| Left stick click | Reserved |
| Left stick up/down for historical year | Deferred; conflicts with locomotion axis and must be assigned a separate gesture/control or used only while a history selector is open |

The left-stick mapping above supersedes the earlier preliminary idea of using the right stick for basic forward movement.

## Rebinding requirements

- Every action exposed in Settings can be rebound at runtime.
- A physical control binding must not be assigned to two different actions in the active control scheme.
- If the selected control is already assigned, reject the new binding and explain the conflict.
- Provide Restore Defaults.
- Persist binding overrides locally on the headset.
- Provide a clear cancel action and timeout for the listen-for-input screen.
- Display the current binding beside each action.

The first prototype script is `VovanderRebindingManager.cs`. It is a foundation only; production UI, composite-binding conflict checks, device-specific binding filters, and restore-defaults UI remain to be implemented and tested.

## Animation

Prefer Meta's built-in runtime model animation. Do not independently rotate the same trigger/button/thumbstick pivots if Meta's runtime prefab already animates them; that would cause double animation. The optional `ControllerInputAnimator.cs` is only for a custom model hierarchy where explicit pivots are intentionally used.

## Input backend

Use Unity Input System for new code. Bind actions through InputActionAssets / InputActionReferences. Meta's current guidance recommends Input System for new projects; OVRInput remains legacy support.
