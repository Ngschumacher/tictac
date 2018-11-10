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
            game : props.game,
            board : props.board,
            winner: null
        }
    }
    


    updateBoardAjax(loc, playerId) {
        if(this.state.gameEnded)
        return;
        console.log(this.props);
        console.log("gameId", this.props.game.gameId)
        let gameId = this.state.game.id;
        playerId =  this.state.game.currentTurn;


        fetch(`http://localhost:5000/api/game/Makemove?gameid=${gameId}&move=${loc}&playerid=${playerId}`)
        .then(result => {
            let json = result.json();
            return json;

        }).then(data => {
            console.log("data is fetched", data);
            
            let playerId =  this.state.game.currentTurn == 
            this.state.game.player1Id ? 
                this.state.game.player2Id :
                this.state.game.player1Id;
            this.setState({
                game : data.game,
                gameBoard : data.board.positions
            })

            if(data.gameEnded){
                this.setState(
                    {
                        winner : data.gameEnded,
                        winnerName : data.winnerName
                    }
                )
            }
        })
    }


    componentDidMount = () => {
        // fetch(`http://localhost:5000/api/game/NewGame`)
        // .then(result => {
        //     return result.json();
        // }).then( data => {
        //     this.setState({
        //         game : data.game,
        //         gameBoard : data.board.positions
        //     });
        // } )

    }

    resetBoard() {
            this.setState({
              
            gameBoard: [
                ' ', ' ', ' ',
                ' ', ' ', ' ',
                ' ', ' ', ' '
            ],
            winner: null
            }) 
    }

    render() {
        return (
            <div className="container">
                <div className="menu">
                    <h1>Tic-Tac-Toe</h1>
                    {/* <Announcement winner={this.state.winner} winnerName={this.state.winnerName} /> */}
                    {/* <ResetButton reset={this.resetBoard.bind(this)}/> */}
                </div>
                
                {this.state.board.positions.map(function(value, i) {
                    return (
                        <Tile 
                        key={i}
                        loc={i}
                        value={value}
                        updateBoard={this.updateBoardAjax.bind(this)}
                        turn={this.state.turn}
                        />
                        );
                }.bind(this))}
            </div>
        );
    }

}


export default Game;