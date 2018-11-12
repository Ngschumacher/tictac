import React, { Component } from 'react';
// import  BoardVertical from './BoardVertical';
// import  Cell from './Cell';
import { HubConnectionBuilder } from '@aspnet/signalr';
import Announcement from './Announcement';
import Button from './Button';
import Tile from './Tile';



class Game extends Component {
    constructor(props) {
        super(props);
        this.state = {
            gameEnded: false
        }
    }
    


    updateBoardAjax(loc) {
        if(this.props.gameStatus.gameEnded)
        return;

        let gameId = this.props.game.id;
        let playerId =  this.props.user.id;

        fetch(`http://localhost:5000/api/game/makemove?gameid=${gameId}&move=${loc}&playerid=${playerId}`)
        .then(result => {
            let json = result.json();
            return json;
        })
    }

    showStats() {
        this.setState({gameEnded : false })
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
                </div>
                <div className={this.props.game.gameStatus.gameEnded ? 'visible' : 'hidden'}>
                    <Button onClick={ this.props.showStats }>Show stats</Button>
                    <Button onClick={ this.props.closeGame }>Close</Button>
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