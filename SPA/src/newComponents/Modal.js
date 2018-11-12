import React, { Component } from 'react';
import AcceptChallengeButton from './AcceptChallengeButton';

class Modal extends Component {
    constructor(props) {
    }

    aceeptChallengeOnClick(gameId) {
        this.props.acceptChallenge(this.props.userId, gameId);
      }

    render() {
        return (
            <div className={this.props.show ? "modal display-block" : "modal display-none" }>
                <section className="modal-main">
                    {this.props.children}
                    <AcceptChallengeButton id={this.props.gameId} onClick={ this.aceeptChallengeOnClick.bind(this) } />
                    <button onClick={this.props.handleClose}>close</button>
                </section>
            </div>
        )
    }
}


export default Modal;