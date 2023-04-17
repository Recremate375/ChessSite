
let cwkChessBoard;
let cwkChessGame;

var renderBoard = function(position, orientation, playingColor) {


    cwkChessGame = new Chess(position);
    
    function onDragStart(source, piece, position, orientation) {
        if(cwkChessGame.game_over()) return false;

        if(playingColor === 'w' &&piece.search(/^b/) !== -1) return false;
        if(playingColor === 'b' &&piece.search(/^w/) !== -1) return false;
    }

    function onDrop(source, target) {
        //if the move is legal
        var move = cwkChessGame.move({
            from: source,
            to: target,
            promotion: 'q'
        });
        
        //illegal move
        if(move === null) return 'snapback';
    }

    function onSnapEnd(){
        cwkChessBoard.position(cwkChessGame.fen());
        let history = cwkChessGame.history();
        let lastMove = history[history.length - 1];
        DotNet.invokeMethodAsync('ChessProject.App', 'AuthenticatedPlayerMoved', lastMove);
    }

    var config = {
        orientation: orientation,
        draggable: true,
        position: position,
        onDragStart: onDragStart,
        onDrop: onDrop,
        onSnapEnd: onSnapEnd
    };

    cwkChessBoard = Chessboard('board', config);
}

function makeOpponentMove(move) {
    cwkChessGame.move(move);
    cwkChessBoard.position(cwkChessGame.fen());

}

function isGameOver(){
    return cwkChessGame.game_over();
}

function getGameResult() {
    if(cwkChessGame.game_over()) {
        if(cwkChessGame.in_checkmate()){
            let winner = getWinner();

            switch(winner) {
                case 'w':
                    return '1-0';
                case 'b': 
                    return '0-1';
            }
        }
        if(cwkChessGame.in_draw()) return '1/2-1/2';
    } else {
        return 'in progress';
    }
}

function getWinner(){ 
    if(cwkChessGame.game_over()) {
        if(cwkChessGame.in_checkmate() && cwkChessGame.turn() === 'w') return 'b';
        else return 'w';
    }
}
