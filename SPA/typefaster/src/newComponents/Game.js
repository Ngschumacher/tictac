import React, { Component } from 'react';
// import  BoardVertical from './BoardVertical';
// import  Cell from './Cell';
import { HubConnectionBuilder } from '@aspnet/signalr';
import Announcement from './Announcement';
import ResetButton from './ResetButton';
import Tile from './Tile';



class Game extends Component {
    constructor(props) {
        super(props);
        console.log(props);
        this.state = {
            user : props.user,
            game : props.game,
            board : props.board,
            winner: null
        }
    }
    


    updateBoardAjax(loc) {
        if(this.state.gameEnded)
        return;

        console.log("game", this.props.game)
        let gameId = this.state.game.id;
        let playerId =  this.state.user.id;

        fetch(`http://localhost:5000/api/game/Makemove?gameid=${gameId}&move=${loc}&playerid=${playerId}`)
        .then(result => {
            let json = result.json();
            return json;

        }).then(data => {
            // console.log("data is fetched", data);
            
            // let playerId =  this.state.game.currentTurn == 
            // this.state.game.player1Id ? 
            //     this.state.game.player2Id :
            //     this.state.game.player1Id;
            // this.setState({
            //     game : data.game,
            //     gameBoard : data.board.positions
            // })

            // if(data.gameEnded){
            //     this.setState(
            //         {
            //             winner : data.gameEnded,
            //             winnerName : data.winnerName
            //         }
            //     )
            // }
        })
    }


    render() {
        return (
            <div className="container">
                <div className="menu">
                    <h1>Tic-Tac-Toe</h1>
                    <Announcement 
                        user={ this.props.user }
                        game={ this.props.game } 
                        gameStatus={ this.props.gameStatus }
                    />
                    {/* <ResetButton reset={this.resetBoard.bind(this)}/> */}
                </div>
                
                {this.props.board.positions.map(function(value, i) {
                    return (
                        <Tile 
                        key={i}
                        loc={i}
                        value={value}
                        updateBoard={this.updateBoardAjax.bind(this)}
                        turn={this.props.turn}
                        />
                        );
                }.bind(this))}
            </div>
        );
    }

}


export default Game;