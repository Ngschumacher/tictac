import React, { Component } from 'react';


class Announcement extends Component {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <div className={this.props.gameStatus.winner ? 'visible' : 'hidden'}>
                    <h2>We've found a winner!!</h2>  
                        <h3 className={this.props.gameStatus.winner && this.props.gameStatus.winner.id == this.props.user.id ? "visible green" : "hidden"  }> You win </h3> 
                        <h3 className={this.props.gameStatus.winner && this.props.gameStatus.winner.id != this.props.user.id ? "visible red" : "hidden"  }> You loose </h3> 
                </div>
                <div className={this.props.gameStatus.gameEnded && !this.props.gameStatus.winner ? 'visible' : 'hidden'}>
                    ooohhh boy. We've got a tie
                </div>
                <div className= {this.props.gameStatus.gameEnded ? 'hidden' : 'visible' }>
                <h3>
                    {this.props.game.whosNext.id == this.props.user.id ? 'Make your move' : 'Waiting for opponent'}
                </h3>
                </div>
            </div>
            
        );
    }
}

export default Announcement;