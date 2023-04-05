
let rcrmChessBoard;
let rcrmChessGame;

function renderBoard(position, orientation, playingColor) {

    import Chess from './chess.js';
    rcrmChessGame = new Chess(position);

    function onDragStart(source, piece, position, orientation) {
        if(rcrmChessGame.game_over()) return false;

        if(playingColor === 'w' &&piece.search(/^b/) !== -1) return false;
        if(playingColor === 'b' &&piece.search(/^w/) !== -1) return false;
    }

    function onDrop(source, target) {
        //if the move is legal
        var move = rcrmChessGame.move({
            from: source,
            to: target,
            promotion: 'q'
        });


        //illegal move
        if(move === null) return 'snapback';
    }

    function onSnapEnd(){
        rcrmChessBoard.position(rcrmChessGame.fen());
        let history = rcrmChessGame.history();
        let lastMove = history[history.length - 1]
        //DotNet.invokeMethodAsync('ChessProject.Blazor', 'AuthenticatedPlayerMoved', lastMove);
    }

    var config = {
        orientation: orientation,
        draggable: true,
        position: position,
        onDragStart: onDragStart,
        onDrop: onDrop,
        onSnapEnd: onSnapEnd
    };

    rcrmChessBoard = ChessBoard('board', config);
}

function makeOpponentMove(move) {
    rcrmChessGame.move(move);
    rcrmChessBoard.position(rcrmChessGame.fen());

}

function isGameOver(){
    return rcrmChessGame.game_over();
}

function getGameResult() {
    if(rcrmChessGame.game_over()) {
        if(rcrmChessGame.in_checkmate()){
            let winner = getWinner();

            switch(winner) {
                case 'w':
                    return '1-0';
                case 'b': 
                    return '0-1';
            }
        }
        if(rcrmChessGame.in_draw()) return '1/2-1/2';
    } else {
        return 'in progress';
    }
}

function getWinner(){ 
    if(rcrmChessGame.game_over()) {
        if(rcrmChessGame.in_checkmate() && rcrmChessGame.turn() === 'w') return 'b';
        else return 'w';
    }
}
