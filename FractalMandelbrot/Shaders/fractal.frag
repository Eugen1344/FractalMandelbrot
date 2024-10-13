#version 410 core

uniform ivec2 screenSize;
uniform vec2 cameraPos;
uniform float scaleFactor;

out vec4 color;

in vec4 Color;

/*const vec4 colors[5] = vec4[5](
vec4(0, 0.0274509803921569, 0.392156862745098, 1), 
vec4(0.1254901960784314, 0.4196078431372549, 0.796078431372549, 1), 
vec4(0.9294117647058824, 1, 1, 1), 
vec4(1, 0.6666666666666667, 0, 1), 
vec4(0, 0.007843137254902, 0, 1));*/

void main()
{
	vec2 newCoord = vec2((gl_FragCoord.x)  * (4.0/screenSize.x) - 2, (gl_FragCoord.y) * (4.0/screenSize.y) - 2) / scaleFactor + cameraPos;

	if(abs(length(newCoord) - 2.0) < 0.01)
	{
		color = vec4(1, 1, 0, 1);
		return;
	}

	float r = sqrt((newCoord.x - 0.25) * (newCoord.x - 0.25) + newCoord.y * newCoord.y);
	float f = atan(newCoord.y, newCoord.x - 0.25);
	float card = 0.5 - 0.5 * cos(f);

	if(r <= card)
	{
		color = vec4(0, 0, 0, 1);
		return;
	}

	dvec2 z = dvec2(0, 0);

	int iterations = 1000;

	int i = 0;
	for(i = 0; i < iterations; i++)
	{
		if(z.x * z.x + z.y * z.y > 4)
			break;

		z = dvec2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + newCoord;
	}

	double size = sqrt(z.x * z.x + z.y * z.y);
	int iterSize = 100;
	float x = float(i % iterSize);

	if(size <= 2)
		color = vec4(0, 0, 0, 1);
	else
	{
		/*double smoothed = log(log(float(size)) * ONE_OVER_LOG2) * ONE_OVER_LOG2;
		int colorI = int((sqrt(i + 1 - smoothed) * 256 + 0)) % 5;
		color = colors[colorI];*/

		color = vec4(vec3(1, 0, 0) * (((x - iterSize) * (x - iterSize/2)) / ((0 - iterSize) * (0 - iterSize/2))) +
		vec3(0, 1, 0) * ((x * (x - iterSize / 2)) / (iterSize * (iterSize - iterSize/2))) + 
		vec3(0, 0, 1) * (((x - iterSize) * (x)) / ((iterSize/2) * (iterSize/2 - iterSize))), 1);
	}
}