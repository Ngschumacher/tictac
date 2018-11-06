import React, { Component } from 'react';


class Announcement extends Component {
    render() {
        return (
            <div className={this.props.winner ? 'visible' : 'hidden'}>
                <h2>Game over {this.props.winnerName} </h2>
            </div>
        );
    }
}

export default Announcement;