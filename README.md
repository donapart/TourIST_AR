# TourIST_AR

## Overview

This project demonstrates an augmented reality (AR) prototype built with Unity. It targets both Android and iOS devices using Unity's AR Foundation framework.

## Requirements

- **Unity**: 2021.3 LTS or newer is recommended. Earlier LTS releases should also work but are untested.
- **Packages**:
  - `AR Foundation` (version 5.0 or later)
  - `ARCore XR Plugin` for Android builds
  - `ARKit XR Plugin` for iOS builds

Make sure these packages are installed via the Unity Package Manager before opening the project.

## Opening the Project

1. Launch Unity Hub.
2. Click **Open** and select this repository's folder.
3. Unity will load the project and resolve the required packages.

## Running the Demo Scene

1. In the Project window, open `Assets/Scenes/Demo.unity` (or your own scene if different).
2. Press **Play** in the Unity editor to test with a connected device or simulator.

## Building for Android

1. Open **File > Build Settings**.
2. Select **Android** and click **Switch Platform** if necessary.
3. Ensure `ARCore XR Plugin` is installed.
4. Configure your Android settings (minimum SDK, package name, etc.).
5. Click **Build** or **Build and Run** to generate the APK.

## Building for iOS

1. Open **File > Build Settings**.
2. Select **iOS** and click **Switch Platform**.
3. Ensure `ARKit XR Plugin` is installed.
4. Click **Build** to generate an Xcode project.
5. Open the project in Xcode, connect your iOS device, and run.

## Folder Structure

- `Assets/` – Unity assets, scenes, and scripts. The core logic for the AR demo lives in `Assets/Scripts/`.
- `Packages/` – Tracks Unity package dependencies, including AR Foundation and platform plugins.
- `ProjectSettings/` – Unity project and build configuration files.

## Key Scripts

- **ARSessionController.cs** – Manages AR session state and handles runtime initialization.
- **ObjectPlacer.cs** – Example script for placing objects in the real world using surface detection.

These scripts are located in `Assets/Scripts/` and are referenced by the demo scene.

