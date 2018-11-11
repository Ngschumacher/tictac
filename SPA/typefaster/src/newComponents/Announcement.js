import React, { Component } from 'react';


class Announcement extends Component {

    constructor(props) {
        super(props);

        console.log(props);
    }

    render() {
        return (
            <div>
            <div className={this.props.gameStatus.winner ? 'visible' : 'hidden'}>
                <h2>Game over  
                    <span className={this.props.gameStatus.winner && this.props.gameStatus.winner.id == this.props.user.id ? "visible green" : "hidden"  }> You win </span> 
                    <span className={this.props.gameStatus.winner && this.props.gameStatus.winner.id != this.props.user.id ? "visible red" : "hidden"  }> You loose</span> 
                </h2>
                
            </div>
            <div className= {this.props.gameStatus.winner ? 'hidden' : 'visible' }>
            <h3>
                {this.props.game.currentTurn == this.props.user.id ? 'Make your move' : 'Waiting for opponent'}
            </h3>
            </div>
            </div>
            
        );
    }
}

export default Announcement;