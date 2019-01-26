#version 410 core

layout (location = 0) in vec2 position;
layout (location = 1) in vec4 color;

out vec4 Color;

void main()
{
	gl_Position = vec4(position.xy, 1, 1);
	Color = color;
}