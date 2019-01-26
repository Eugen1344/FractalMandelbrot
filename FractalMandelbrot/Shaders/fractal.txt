#version 410 core

uniform ivec2 screenSize;
uniform dvec2 cameraPos;
uniform double scaleFactor;

out vec4 color;

in vec4 Color;

const float ONE_OVER_LOG2 = 1.4426950408889634073599246810019;

const vec4 colors[5] = vec4[5](
vec4(0, 0.0274509803921569, 0.392156862745098, 1), 
vec4(0.1254901960784314, 0.4196078431372549, 0.796078431372549, 1), 
vec4(0.9294117647058824, 1, 1, 1), 
vec4(1, 0.6666666666666667, 0, 1), 
vec4(0, 0.007843137254902, 0, 1));

double atan2(double y, double x)
{
    double atan_tbl[10];
    atan_tbl[0] = -3.333333333333333333333333333303396520128e-1LF;
    atan_tbl[1] =  1.999999117496509842004185053319506031014e-1LF;
  atan_tbl[2] =   -1.428514132711481940637283859690014415584e-1LF;
 atan_tbl[3] =     1.110012236849539584126568416131750076191e-1LF;
atan_tbl[4] =     -8.993611617787817334566922323958104463948e-2LF;
atan_tbl[5] =      7.212338962134411520637759523226823838487e-2LF;
 atan_tbl[6] =    -5.205055255952184339031830383744136009889e-2LF;
atan_tbl[7] =      2.938542391751121307313459297120064977888e-2LF;
 atan_tbl[8] =    -1.079891788348568421355096111489189625479e-2LF;
 atan_tbl[9] =     1.858552116405489677124095112269935093498e-3LF;

    /* argument reduction: 
       arctan (-x) = -arctan(x); 
       arctan (1/x) = 1/2 * pi - arctan (x), when x > 0
    */

    double ax = abs(x);
    double ay = abs(y);
    double t0 = max(ax, ay);
    double t1 = min(ax, ay);
    
    double a = 1 / t0;
    a *= t1;

    double s = a * a;
    double p = atan_tbl[9];

    p = fma( fma( fma( fma( fma( fma( fma( fma( fma( fma(p, s,
        atan_tbl[8]), s,
        atan_tbl[7]), s, 
        atan_tbl[6]), s,
        atan_tbl[5]), s,
        atan_tbl[4]), s,
        atan_tbl[3]), s,
        atan_tbl[2]), s,
        atan_tbl[1]), s,
        atan_tbl[0]), s*a, a);

    double r = ay > ax ? (1.57079632679489661923LF - p) : p;

    r = x < 0 ?  3.14159265358979323846LF - r : r;
    r = y < 0 ? -r : r;

    return r;
}

double sina_11(double x)
{
    //minimax coefs for sin for 0..pi/2 range
    const double a3 = -1.666666660646699151540776973346659104119e-1LF;
    const double a5 =  8.333330495671426021718370503012583606364e-3LF;
    const double a7 = -1.984080403919620610590106573736892971297e-4LF;
    const double a9 =  2.752261885409148183683678902130857814965e-6LF;
    const double ab = -2.384669400943475552559273983214582409441e-8LF;

    const double m_2_pi = 0.636619772367581343076LF;
    const double m_pi_2 = 1.57079632679489661923LF;

    double y = abs(x * m_2_pi);
    double q = floor(y);
    int quadrant = int(q);

    double t = (quadrant & 1) != 0 ? 1 - y + q : y - q;
    t *= m_pi_2;

    double t2 = t * t;
    double r = fma(fma(fma(fma(fma(ab, t2, a9), t2, a7), t2, a5), t2, a3),
        t2*t, t);

    r = x < 0 ? -r : r;

    return (quadrant & 2) != 0 ? -r : r;
}

double cosa_11(double x)
{
    //sin(x + PI/2) = cos(x)
    return sina_11(x + 1.57079632679489661923LF);
}

void main()
{
	dvec2 newCoord = dvec2((gl_FragCoord.x)  * (4.0/screenSize.x) - 2, (gl_FragCoord.y) * (4.0/screenSize.y) - 2) / scaleFactor + cameraPos;

	if(abs(length(newCoord) - 2.0) < 0.01)
	{
		color = vec4(1, 1, 0, 1);
		return;
	}

	double r = sqrt((newCoord.x - 0.25) * (newCoord.x - 0.25) + newCoord.y * newCoord.y);
	double f = atan2(newCoord.y, newCoord.x - 0.25);
	double card = 0.5 - 0.5 * cosa_11(f);

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