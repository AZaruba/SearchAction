shader_type canvas_item;
uniform sampler2D Texture : filter_nearest;
void fragment() {
	COLOR = texture(Texture, UV);
}