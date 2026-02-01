shader_type spatial;
render_mode unshaded;
uniform sampler2D Texture2DParameter : filter_linear;

void fragment() {
    ALBEDO = texture(Texture2DParameter, UV).rgb;
}