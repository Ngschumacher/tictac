import React, { Component } from 'react';

class AcceptChallengeButton extends Component {
    render() {
        return (
            <button  onClick={() => this.props.onClick(this.props.id)}>Accept</button>
        )
    }
}


export default AcceptChallengeButton;