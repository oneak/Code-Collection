from PIL import Image

tile_w, tile_h = 128, 128  # Set the width and height for each tile in the atlas

atlas = Image.new('RGBA', (tile_w*3, tile_h*2))  # Create a blank atlas image with 3 columns and 2 rows

for i in range(6):
    tile = Image.open(f'tile_{i+1}.png')  # Open tile image (expects files named tile_1.png to tile_6.png)
    # Resize the tile if it is not already 128x128 pixels
    if tile.size != (tile_w, tile_h):
        tile = tile.resize((tile_w, tile_h), Image.LANCZOS)
    x = (i % 3) * tile_w  # Calculate the x position (column) for this tile
    y = (i // 3) * tile_h  # Calculate the y position (row) for this tile
    atlas.paste(tile, (x, y))  # Paste the tile into the correct position in the atlas

atlas.save('output.png')  # Save the final atlas image as output.png