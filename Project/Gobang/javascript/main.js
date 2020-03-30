blipp = require('blippar').blipp;

// ======================================================
// =                        BLIPP                       =
// ======================================================
blipp.getPeel()
	 .setOrientation("portrait")
	 .setType("fit");

// ======================================================
// =                        SCENE                       =
// ======================================================
//blipp.read("main.json");

var scene   = blipp.addScene("default");
var scene2    = blipp.addScene("scene2");

var markerW = blipp.getMarker().getWidth();
var markerH = blipp.getMarker().getHeight();

var music_id;
scene.onCreate = function() 
{
	scene.addSprite("Table.jpg")
		 .setName("background")
		 .setScale(markerW, markerH, 1)
		 .setClickable(false);

	var logo = scene.addSprite("Title.png")
					.setName("logo")
					.setClickable(true);
					
	scene.soundFiles = ["startScen.mp3"];				
	// Define the list of required assets to be downloaded
	scene.setRequiredAssets(scene.soundFiles);				
	scene.playSound("startScen.mp3",true,music_id,1,1,1);							
	
	logo.onCreate = function()
	{
		this.setTranslation(-225, 0, 1);
		this.animationStartTx   = -225;
		this.animationEndTx     = 0;
		this.animationStartRz   = 0;
		this.animationEndRz     = 3600;
		this.animationStartSc   = 1;
		this.animationEndSc     = 580;
		this.animationStartTime = -1;
		this.animationDuration  = 1000;
	}

	logo.onTouchEnd = function(id, x, y)
	{
		scene.stopSounds();
		blipp.goToScene(scene2);
	    console.log("Touched on : " + this.getName());
	}

	logo.onUpdate = function(time)
	{
		animateLogo(this, time);
	}
}

scene.onShow = function()
{
    console.log("scene.onShow");
}

scene2.onCreate = function () {
	// Define the background space
//	var background = scene2.getBackground();
	scene2.addSprite("Table.jpg")
				.setName("Table")
				.setScale(markerW, markerH, 1)
				.setClickable(false);
	
	scene2.soundFiles = ["startScen1.mp3","pass.mp3","fail.mp3"];				
	// Define the list of required assets to be downloaded
	scene2.setRequiredAssets(scene2.soundFiles);				
	scene2.playSound("startScen1.mp3",true,music_id,1,1,1);				
				
	// Scene global variables
	scene2.gameTurn		= 0;
	scene2.tilesEdgeSize = 100;
	scene2.tilesGapSize 	= 50;			

	scene2.tiles 		= [];
	scene2.tilesSymbols 	= [];
	scene2.tileValue		= [];
	
	var tilesSize = scene2.tilesEdgeSize + scene2.tilesGapSize;
	for (x = -2; x < 3; x++) {
		for (y = -2; y < 3; y++) {
			var index = (y+2) + ((x+2)*5);
			var	tile  = scene2.addSprite()
							 .setName("Tile_" + index)
							 .setTexture('Box1.png')
							 .setTranslation(x * tilesSize ,y * tilesSize, 100)
							 .setScale(scene2.tilesEdgeSize)
							 .setClickable(false);
		tile.index = index;
		tile.onTouchEnd = function(id, x, y) 
		{
			this.setScale(0);
			playerMove(this.index);
		}	

		var symbol = scene2.addMesh('Tumbler.md2')//.addSprite()
							  .setName("Symbol_" + index)
							  .setTexture('Tumbler.jpg')
							  .setTranslation(x * tilesSize, y * tilesSize, 25)
							  .setRotation(90, 0, 30)
							  .setType('hider')
							  .setClickable(false);		
			scene2.tiles.push(tile);
			scene2.tilesSymbols.push(symbol);
		}
	}
		// Create end-of-game panel
	scene2.endPanel= scene2.addSprite()
						 .setTextures(["end_draw.png","end_X_win.png","end_O_win.png"])
						 .setScale(0)
						 .setTranslation(0,0,300)
			   			 .setType('phantom')
						 .setClickable(false);
	scene2.endPanel.onTouchEnd = function(id, x, y) {
		scene2.stopSounds();
		this.setClickable(false);
		this.animate().scale(0).duration(200);	
		blipp.goToScene(scene);
	}
	beginGame();
}

scene2.onShow = function()
{
    console.log("scene2.onShow");
}

function beginGame() {
	scene2.gameTurn = 0;
	scene2.tileValue = [' ', ' ', ' ', ' ', ' ',
					 ' ', ' ', ' ', ' ', ' ',
					 ' ', ' ', ' ', ' ', ' ',
					 ' ', ' ', ' ', ' ', ' ',
					 ' ', ' ', ' ', ' ', ' '];
					 
	console.log("beginGame" + scene2.tiles.length);
	
	for (var i = 0; i < scene2.tiles.length; i++) 
	{
		scene2.tiles[i].setClickable(true);	
		scene2.tilesSymbols[i].animate().rotationZ(30).duration(500);
	}

}

function playerMove(tileIndex) {
	// Mark the selected tile with an X
	if (scene2.gameTurn % 2 == 0) {
		scene2.tileValue[tileIndex] = 'X';
		scene2.tilesSymbols[tileIndex].animate().rotationZ(30 - 120).duration(500);
		tilesAllowClick(false);
	}
	// Mark turn as played
	scene2.gameTurn++;

	// If player did not win here, it's computer turn
	if ( ! checkForWinner()) {
		scene2.animate().duration(500).onEnd = function () 
		{ 
			computerMove();
		}
	}
}

function computerMove() {
	// Choose a tile to play
	var tileIndex = computerPickTile();

	scene2.tilesSymbols[tileIndex].animate().rotationZ(30 + 120).duration(500);
	scene2.tileValue[tileIndex] = 'O';
	scene2.tiles[tileIndex].setScale(0);
	
	// Mark turn as played
	scene2.gameTurn++;
	
	// Check if player won or game ended with a draw
	scene2.animate().duration(500).onEnd = function () { 
		checkForWinner();
		tilesAllowClick(true);
	}
}

function computerPickTile() {
	// Check if 2 symbols are already aligned and pick the 3rd tile
	if (isLineOf4(1, 2, 3, 4, 0) || isLineOf4(5, 10, 15, 20 , 0) || isLineOf4( 6 ,12, 18, 24 , 0)) return 0;
	if (isLineOf4(6, 11, 16, 21, 1))      return 1;
	if (isLineOf4(7, 12, 17, 22 , 2)) 	return 2;
	if (isLineOf4( 8, 13, 18, 23, 3 )) 	return 3;
	if (isLineOf4(9, 14, 19, 24 , 4) || isLineOf4( 8, 12, 16, 20, 4)) return 4;
	if (isLineOf4(6, 7, 8,  9, 5))     return 5;
	if (isLineOf4(5, 7, 8,  9, 6) || isLineOf4( 0 ,12, 18, 24 , 6) || isLineOf4(1, 11, 16, 21, 6))     return 6;
	if (isLineOf4(5, 6, 8,  9, 7) || isLineOf4(2, 12, 17, 22, 7))  return 7;
	if (isLineOf4(3, 13, 18, 23, 8) || isLineOf4(5, 6, 7,  9, 8))   return 8;
	if (isLineOf4(4, 14, 19, 24, 9) || isLineOf4(5, 6, 7,  8, 9))   return 9;
	if (isLineOf4(11, 12, 13, 14, 10) || isLineOf4(0 ,5, 15, 20 , 10))  return 10;
	if (isLineOf4(10, 12, 13, 14, 11) || isLineOf4(1 ,6, 16, 21 , 11))  return 11;
	if (isLineOf4(10, 11, 13, 14, 12) || isLineOf4(2 ,7, 17, 22 , 12))  return 12;
	if (isLineOf4(10, 11, 12, 14, 13) || isLineOf4(3 ,8, 18, 23 , 13))  return 13;
	if (isLineOf4(10, 11, 12, 13, 14) || isLineOf4(4 ,9, 19, 24 , 14))  return 14;
	if (isLineOf4(16, 17, 18, 19, 15))  return 15;
	if (isLineOf4(21, 22, 23, 24, 20) ) return 20;
	
	// Pick an empty tile
	if (scene2.tileValue[12] == ' ') 
		return 12;
	if (scene2.tileValue[8] == ' ') 
		return 8;
	if (scene2.tileValue[6] == ' ') 
		return 6;
	if (scene2.tileValue[18] == ' ')
		return 18;
	if (scene2.tileValue[16] == ' ')
		return 16;
	if (scene2.tileValue[4] == ' ') 
		return 4;
	if (scene2.tileValue[0] == ' ') 
		return 0;
	if (scene2.tileValue[24] == ' ') 
		return 24;
	if (scene2.tileValue[20] == ' ') 
		return 20;
	if (scene2.tileValue[3] == ' ') 
		return 3;
	if (scene2.tileValue[13] == ' ') 
		return 13;
	if (scene2.tileValue[23] == ' ')
		return 23;
	if (scene2.tileValue[1] == ' ')
		return 1;
	if (scene2.tileValue[11] == ' ') 
		return 11;
	if (scene2.tileValue[21] == ' ') 
		return 21;
	if (scene2.tileValue[14] == ' ') 
		return 14;
	
	if (scene2.tileValue[10] == ' ') 
		return 10;
	if (scene2.tileValue[9] == ' ') 
		return 9;
	if (scene2.tileValue[19] == ' ') 
		return 19;
	if (scene2.tileValue[5] == ' ') 
		return 5;
	if (scene2.tileValue[15] == ' ') 
		return 15;
	if (scene.tileValue[7] == ' ')
		return 7;
	if (scene2.tileValue[17] == ' ')
		return 17;
	if (scene2.tileValue[22] == ' ') 
		return 22;

	return 2;
}

function isLineOf4(i, j, k, m, n) {
	return (scene2.tileValue[i] == 'X' && scene2.tileValue[j] == 'X' && 
		   scene2.tileValue[k] == 'X' && scene2.tileValue[m] == 'X' && 
		   scene2.tileValue[n] == ' ') 
		|| (scene2.tileValue[i] == 'O' && scene2.tileValue[j] == 'O' && 
		   scene2.tileValue[k] == 'O' && scene2.tileValue[m] == 'O' && 
		   scene2.tileValue[n] == ' ');
}


function checkForWinner() {
	var winner = '';

	if (isLineOf5(0, 1, 2,  3,  4) == 'X' 
	 || isLineOf5(0, 5, 10, 15, 20) == 'X' 
	 || isLineOf5(0, 6 ,12, 18, 24) == 'X' 
	 || isLineOf5(1, 6, 11, 16, 21) == 'X' 
	 || isLineOf5(2, 7, 12, 17, 22) == 'X' 
	 || isLineOf5(3, 8, 13, 18, 23) == 'X' 
	 || isLineOf5(4, 9, 14, 19, 24) == 'X' 
	 || isLineOf5(4, 8, 12, 16, 20) == 'X' 
	 || isLineOf5(5, 6,  7, 8,  9) == 'X'
	 || isLineOf5(10,11, 12, 13, 14) == 'X'
	 || isLineOf5(15,16, 17, 18, 19) == 'X'
	 || isLineOf5(20,21, 22, 23, 24) == 'X') 
	 {
		winner = 'X';
	} 
	else 
	{
		if (isLineOf5(0, 1, 2,  3,  4) == 'O' 
		|| isLineOf5(0, 5, 10, 15, 20) == 'O' 
		|| isLineOf5(0, 6 ,12, 18, 24) == 'O' 
		|| isLineOf5(1, 6, 11, 16, 21) == 'O' 
		|| isLineOf5(2, 7, 12, 17, 22) == 'O' 
		|| isLineOf5(3, 8, 13, 18, 23) == 'O' 
		|| isLineOf5(4, 9, 14, 19, 24) == 'O' 
		|| isLineOf5(4, 8, 12, 16, 20) == 'O' 
		|| isLineOf5(5, 6,  7, 8,  9) == 'O'
		|| isLineOf5(10,11, 12, 13, 14) == 'O'
		|| isLineOf5(15,16, 17, 18, 19) == 'O'
		|| isLineOf5(20,21, 22, 23, 24) == 'O' )
		{
				winner = 'O';
		} 
		else 
		{
			if (scene2.gameTurn == scene2.tiles.length) 
			{				
				winner = ' ';
			}
		}
	}
	

	if (winner != '' ) {				
		endGame(winner);
		return(true);
	}

	return(false);
}

function endGame(winner) {
	var textureIndex = 0;		// DRAW

	switch (winner) {
		case 'X':
			textureIndex = 1;	// X 
			scene2.stopSounds();
			scene2.playSound("pass.mp3");
			break;
		case 'O':
			textureIndex = 2;	// O
			scene2.stopSounds();
			scene2.playSound("fail.mp3");
			break;
	}

	scene2.endPanel.setActiveTexture(textureIndex);
	scene2.endPanel.animate().scale(800).duration(200);
	for(i = 0; i < scene2.tiles.length; i++){
		scene2.tiles[i].setClickable(false);
	}

	scene2.endPanel.setClickable(true);	

}

function isLineOf5(i, j, k, m, n) {
	if (scene2.tileValue[i] == scene2.tileValue[j] && 
	    scene2.tileValue[j] == scene2.tileValue[k] && 
		scene2.tileValue[k] == scene2.tileValue[m] &&
		scene2.tileValue[m] == scene2.tileValue[n]) 
		{
		if (scene2.tileValue[i] == 'X') return 'X';
		if (scene2.tileValue[i] == 'O') return 'O';
	} 
	return ' ';
}

function tilesAllowClick(flag) {
	for (i = 0; i < scene2.tiles.length; i++) {
		scene2.tiles[i].setClickable(flag);
	}
}
// ======================================================
// =                    USER FUNCTIONS                  =
// ======================================================
function animateLogo(node, time)
{
	if (node.animationStartTime == -1) {
		node.animationStartTime = time;
	}
	if ((time - node.animationStartTime) >= node.animationDuration) {
		return;
	}

	var interp = (time - node.animationStartTime) / node.animationDuration;
	var newTranslation = node.getTranslation();
	newTranslation[0]  = ((node.animationEndTx - node.animationStartTx) * interp) + node.animationStartTx;

	var newRotation    = node.getRotation();
//	newRotation[2]     = ((node.animationEndRz - node.animationStartRz) * interp) + node.animationStartRz;

	var newScale 	   = node.getScale();
	newScale           = ((node.animationEndSc - node.animationStartSc) * interp) + node.animationStartSc;

	node.setTranslation(newTranslation);
	node.setRotation(newRotation);
	node.setScale(newScale);
}
