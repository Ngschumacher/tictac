import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';
import ChallengeButton from './ChallengeButton';
import AcceptChallengeButton from './AcceptChallengeButton';

class Chat extends Component {
  constructor(props) {
    super(props);
    console.log("connectedUsers", this.props.connectedUsers);
    this.state = {
      message: '',
      messages: [],
    };
  }

  sendChallenge(id) {
    let challengerId = this.props.user.id;
    let opponentId = id;

    fetch(`http://localhost:5000/api/game/SendChallenge?challengerId=${challengerId}&opponentId=${opponentId}`)
    .then(result => {
    })
  }

  challengeButton(userId){
    if(userId !== this.props.user.id) {
      var challengeButton =  <ChallengeButton id={userId} onClick={ this.sendChallenge.bind(this) }  />            
  }
  }

  render() {
    return (
      <div>
        <br />
        <input
          type="text"
          value={this.state.message}
          onChange={e => this.setState({ message: e.target.value })}
        />

        <button onClick={this.sendMessage}>Send</button>

        <div>
          {this.state.messages.map((message, index) => (
            <span style={{display: 'block'}} key={index}> {message} </span>
          ))}
        </div>
        <div style={UserListStyle}>
          <h3>User list</h3>
          <ul>
             {this.props.connectedUsers.map(function(user, index) {
              
               return ( 
               <li key={ index }>
                  { this.challengeButton(user.id) }
                  { user.username }
               </li>
               );
             }.bind(this))}
          </ul>
        </div>
      </div>
    );
  }
}

const UserListStyle = {
  float: 'right',
  width: '200px',
}

export default Chat;