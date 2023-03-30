using System;
using UnityEngine;

namespace MultiMouse
{
    public class MultiMouseDevice
    {
        private bool isActive;

        public Vector2 Position { get; set; }

        public Vector2 Delta { get; set; }

        public Vector2 ScrollDelta { get; set; }

        public int ButtonCount => this.Buttons.Length;

        private bool[] Buttons { get; }

        private bool[] LastButtons { get; }

        public string Name { get; }

        public IntPtr Handle { get; }

        public bool IsActive
        {
            get => this.isActive;
            set => this.isActive |= value;
        }

        public MultiMouseDevice(IntPtr handle, string name, int buttonCount)
        {
            this.Handle = handle;
            this.Name = name;
            this.Buttons = new bool[buttonCount];
            this.LastButtons = new bool[buttonCount];
        }

        public bool GetMouseButtonDown(int buttonId) => this.Buttons[buttonId] && !this.LastButtons[buttonId];

        public bool GetMouseButtonUp(int buttonId) => !this.Buttons[buttonId] && this.LastButtons[buttonId];

        public bool GetMouseButton(int buttonId) => this.Buttons[buttonId];

        public void SetButtonState(int buttonId, bool state) => this.Buttons[buttonId] = state;

        public void SetLastButtonState(int buttonId, bool state) => this.LastButtons[buttonId] = state;
    }
}
