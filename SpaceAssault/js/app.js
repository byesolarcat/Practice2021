var requestAnimFrame = (function(){
    return window.requestAnimationFrame       ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame    ||
        window.oRequestAnimationFrame      ||
        window.msRequestAnimationFrame     ||
        function(callback){
            window.setTimeout(callback, 1000 / 60);
        };
})();

var canvas = document.createElement("canvas");
var ctx = canvas.getContext("2d");
canvas.width = 512;
canvas.height = 480; 
document.body.appendChild(canvas);

var lastTime;
function main() {
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    update(dt);
    render();

    lastTime = now;
    requestAnimationFrame(main);
}

function init() { 
    terrainPattern = ctx.createPattern(resources.get('img/terrain.png'), 'repeat');

    document.getElementById('play-again').
        addEventListener('click', function () {
            reset();
        });
    reset();
    lastTime = Date.now();
    main();
}

resources.load([
    'img/sprites_02.png',
    'img/terrain.png',
]);
resources.onReady(init);

var player = {
    pos: [0, 0],
    sprite: new Sprite('img/sprites_02.png', [0, 0], [39, 39], 16, [0, 1])
};

var playerSpeed = 200;
var bulletSpeed = 500;
var enemySpeed = 100;

var bullets = [];
var enemies = [];
var explosions = [];
var megaliths = [];
var manna = [];
var eatenManna = [];

var lastFire = Date.now();
var gameTime = 0;
var isGameOver;
var terrainPattern;

var score = 0;
var scoreEl = document.getElementById('score');



function placeMegaliths() {
    var megalithsNum = 3 + Math.random() * (5 + 1 - 3);
    megalithsNum = Math.floor(megalithsNum);

    for(var i = 0; i < megalithsNum; i++) {
        var spritepos = [];
        var isColliding = false;
        var megalithPos = [150 + Math.random() * (canvas.width - 200),
            Math.random() * (canvas.height - 50)];
        var megalithSize = [55, 58];
        if(Math.random() > 0.5) {
            spritepos = [3, 213]
        } else {
            spritepos = [3, 272]
        }
        for(var i=0; i<megaliths.length; i++) {
            if(boxCollides(megalithPos, megalithSize, megaliths[i].pos, megaliths[i].sprite.size)) {
                isColiding = true;
            }
        }
        if (!isColliding) {
            megaliths.push({
                pos: megalithPos,
                sprite: new Sprite('img/sprites_02.png', spritepos, megalithSize, 0, [1, 0], 'vertical')
            });
        }
    }
}

var mannaScore = 0;
var mannaScoreEl = document.getElementById('mannascore');

var mannaEaten = [];

function placeManna() {
    if (manna.length < 5) {
        var isColiding = false;
        var mannapos = [Math.random() * (canvas.width - 50), Math.random() * (canvas.height - 52)];
        for(var i=0; i<megaliths.length; i++) {
            if(boxCollides(mannapos, [50, 52], megaliths[i].pos, megaliths[i].sprite.size)) {
                isColiding = true;
            }
        }
        if(boxCollides(mannapos, [200, 200], player.pos, player.sprite.size)) {
            isColiding = true;
        }
        if(!isColiding){
            manna.push({
                pos: mannapos,
                sprite: new Sprite('img/sprites_02.png', [3, 158], [50, 52],
                                3, [0, 1])
            });
        }
    }
}

function update(dt) {
    gameTime += dt;

    handleInput(dt);
    updateEntities(dt);

    if(Math.random() < 1 - Math.pow(.993, gameTime)) {
        enemies.push({
            pos: [canvas.width,
                  Math.random() * (canvas.height - 39)],
            sprite: new Sprite('img/sprites_02.png', [0, 78], [80, 39],
                               6, [0, 1, 2, 3, 2, 1])
        });
    }

    placeManna();
    
    checkCollisions(dt);

    scoreEl.innerHTML = "Score: " + score;
    mannaScoreEl.innerHTML = "Manna: " + mannaScore;
}

function updateEntities(dt) {
    player.sprite.update(dt);

    for(var i = 0; i < bullets.length; i++) {
        var bullet = bullets[i];
        switch(bullet.dir) {
            case 'up': bullet.pos[1] -= bulletSpeed * dt; break;
            case 'down': bullet.pos[1] += bulletSpeed * dt; break;
            default: bullet.pos[0] += bulletSpeed * dt;
        }

        if(bullet.pos[1] < 0 || 
            bullet.pos[1] > canvas.height ||
            bullet.pos[0] > canvas.width) {
                bullets.splice(i, 1);
                i--;
        }
    }

    for(var i = 0; i < enemies.length; i++) {
        var isColliding = false;
        var megalithPos;
        for(var j = 0; j < megaliths.length; j++) {
            if (boxCollides(enemies[i].pos, enemies[i].sprite.size, megaliths[j].pos, megaliths[j].sprite.size)) {
                isColliding = true;
                megalithPos = megaliths[j].pos;
            }
        }
        if(isColliding && enemies[i].pos[1] > megalithPos[1]) {
            enemies[i].pos[1] += enemySpeed * dt;
        } else if(isColliding && enemies[i].pos[1] <= megalithPos[1]) {
            enemies[i].pos[1] -= enemySpeed * dt; 
        } else {
            enemies[i].pos[0] -= enemySpeed * dt;
        }
        enemies[i].sprite.update(dt);

        if(enemies[i].pos[0] + enemies[i].sprite.size[0] < 0) {
            enemies.splice(i, 1);
        }
    }

    for(var i = 0; i < explosions.length; i++) {
        explosions[i].sprite.update(dt);

        if(explosions[i].sprite.done) {
            explosions.splice(i, 1);
            i--;
        }
    }

    for(var i = 0; i < mannaEaten.length; i++) {
        mannaEaten[i].sprite.update(dt);

        if(mannaEaten[i].sprite.done) {
            mannaEaten.splice(i, 1);
            i--;
        }
    }

    for(var i = 0; i < megaliths.length; i++) {
        megaliths[i].sprite.update(dt);
    }

    for(var i = 0; i < manna.length; i++) {
        manna[i].sprite.update(dt);
    }
}


function handleInput(dt) {
    if(input.isDown('DOWN') || input.isDown('s')) {
        player.pos[1] += playerSpeed * dt;
    }

    if(input.isDown('UP') || input.isDown('w')) {
        player.pos[1] -= playerSpeed * dt;
    }

    if(input.isDown('LEFT') || input.isDown('a')) {
        player.pos[0] -= playerSpeed * dt;
    }

    if(input.isDown('RIGHT') || input.isDown('d')) {
        player.pos[0] += playerSpeed * dt;
    }

    if(input.isDown('SPACE') && !isGameOver && Date.now() - lastFire > 100) {
        var x = player.pos[0] + player.sprite.size[0] / 2;
        var y = player.pos[1] + player.sprite.size[1] / 2;

        bullets.push({ pos: [x, y],
                        dir: 'forward',
                        sprite: new Sprite('img/sprites_02.png', [0, 39], [18, 8])
                        });
        bullets.push({ pos: [x, y],
                        dir: 'up',
                        sprite: new Sprite('img/sprites_02.png', [0, 50], [9, 5])
                        });
        bullets.push({ pos: [x, y],
                        dir: 'down',
                        sprite: new Sprite('img/sprites_02.png', [0, 60], [9, 5])
                        });

        lastFire = Date.now();
    }
}

function collides(x, y, r, b, x2, y2, r2, b2) {
    return !(r <= x2 || x > r2 ||
             b <= y2 || y > b2);
}

function boxCollides(pos, size, pos2, size2) {
    return collides(pos[0], pos[1],
                    pos[0] + size[0], pos[1] + size[1],
                    pos2[0], pos2[1],
                    pos2[0] + size2[0], pos2[1] + size2[1]);
}

function checkCollisions(dt) {
    checkPlayerBounds();

    for(var i=0; i<enemies.length; i++) {
        var pos = enemies[i].pos;
        var size = enemies[i].sprite.size;

        for(var j=0; j<bullets.length; j++) {
            var pos2 = bullets[j].pos;
            var size2 = bullets[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                enemies.splice(i, 1);
                i--;

                score += 100;

                explosions.push({
                    pos: pos,
                    sprite: new Sprite('img/sprites_02.png',
                                       [0, 117],
                                       [39, 39],
                                       16,
                                       [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                                       null,
                                       true)
                });

                bullets.splice(j, 1);
                break;
            }
        }

        if(boxCollides(pos, size, player.pos, player.sprite.size)) {
            gameOver();
        }
    }

    for(var i=0; i<megaliths.length; i++) {
        var pos3 = megaliths[i].pos;
        var size3 = megaliths[i].sprite.size;

        if(boxCollides(pos3, size3, player.pos, player.sprite.size)) {
            if(input.isDown('DOWN') || input.isDown('s')) {
                player.pos[1] -= playerSpeed * dt;
            }
        
            if(input.isDown('UP') || input.isDown('w')) {
                player.pos[1] += playerSpeed * dt;
            }
        
            if(input.isDown('LEFT') || input.isDown('a')) {
                player.pos[0] += playerSpeed * dt;
            }
        
            if(input.isDown('RIGHT') || input.isDown('d')) {
                player.pos[0] -= playerSpeed * dt;
            }
        }

        for(var j=0; j < bullets.length; j++) {
            var pos4 = bullets[j].pos;
            var size4 = bullets[j].sprite.size;

            if(boxCollides(pos3, size3, pos4, size4)) {
                bullets.splice(j, 1);
            }
        }
    }

    for(var i=0; i<manna.length; i++) {
        var pos5 = manna[i].pos;
        var size5 = manna[i].sprite.size;

        if(boxCollides(pos5, size5, player.pos, player.sprite.size)) {
            mannaScore++;
            mannaEaten.push({
                    pos: pos5,
                    sprite: new Sprite('img/sprites_02.png', [3, 158], [50, 52],
                                    4, [0, 1, 2, 3], 'horizontal', true)
                });
            manna.splice(i, 1);
        }
    }
}

function checkPlayerBounds() {
    if(player.pos[0] < 0) {
        player.pos[0] = 0;
    }
    else if(player.pos[0] > canvas.width - player.sprite.size[0]) {
        player.pos[0] = canvas.width - player.sprite.size[0];
    }

    if(player.pos[1] < 0) {
        player.pos[1] = 0;
    }
    else if(player.pos[1] > canvas.height - player.sprite.size[1]) {
        player.pos[1] = canvas.height - player.sprite.size[1];
    }
}

function render() {
    ctx.fillStyle = terrainPattern;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    if(!isGameOver) {
        renderEntity(player);
    }

    renderEntities(mannaEaten);
    renderEntities(bullets);
    renderEntities(enemies);
    renderEntities(explosions);
    renderEntities(megaliths);
    renderEntities(manna);
    renderEntities(eatenManna);
}

function renderEntities(list) {
    for(var i = 0; i < list.length; i++) {
        renderEntity(list[i]);
    }
}

function renderEntity(entity) {
    ctx.save();
    ctx.translate(entity.pos[0], entity.pos[1]);
    entity.sprite.render(ctx);
    ctx.restore();
}

function gameOver() {
    document.getElementById('game-over').style.display = 'block';
    document.getElementById('game-over-overlay').style.display = 'block';
    isGameOver = true;
    mannaScore = 0;
}

function reset() {
    document.getElementById('game-over').style.display = 'none';
    document.getElementById('game-over-overlay').style.display = 'none';
    isGameOver = false;
    gameTime = 0;
    score = 0;

    enemies = [];
    bullets = [];

    megaliths.splice(0, megaliths.length);
    manna.splice(0, manna.length);

    placeMegaliths();


    player.pos = [50, canvas.height / 2];
};