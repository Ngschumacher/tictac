import React, { Component } from 'react';

class ChallengeButton extends Component {
    render() {
        return (
            <button  onClick={() => this.props.onClick(this.props.id)}>challenge</button>
        )
    }

}


export default ChallengeButton;