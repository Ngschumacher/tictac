import React, { Component } from 'react';


class GameStats extends Component {

    constructor(props) {
    }

    getWinner(ended, winner) {
        console.log(winner);
        if(ended) {
            if(winner === null)
            {
                return (<span class="gray">Draw</span>);
            }
            if(winner.id == this.props.user.id) {
                return (
                    <span className="green">Win</span>
                );
            } else {
                return (
                    <span className="red">Lose</span>
                );
            }
            
        }
    return (<span>Not finished</span>);
    }

    render() {
        return (
            <div>
               <table>
                   <thead>
                    <tr>
                        <th>
                            Challenger
                        </th>
                        <th>
                            Opponent
                        </th>
                        <th>
                            Game status
                        </th>
                        <th>
                            Result
                        </th>
                    </tr>
                    </thead>
                    {this.props.gameStats.map(function(game, i) {
                    return (
                        <tr key={i}> 
                            <td>
                                {game.player1.username}
                            </td>
                            <td>
                                {game.player2.username}
                            </td>
                            <td>
                                {game.ended ? "Finished" : "Unfinished"}
                            </td>
                            <td>
                                {this.getWinner(game.ended, game.winner)}
                            </td>
                        </tr>
                        );
                }.bind(this))}
               </table>
            </div>
            
        );
    }
}

export default GameStats;