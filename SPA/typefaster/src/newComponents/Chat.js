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

  componentDidMount = () => {
     
  };

  sendChallenge(id) {
    let challengerId = this.props.user.id;
    let opponentId = id;

    fetch(`http://localhost:5000/api/game/SendChallenge?challengerId=${challengerId}&opponentId=${opponentId}`)
    .then(result => {
    })
  }
  


  

  sendMessage = () => {
    this.state.hubConnection
      .invoke('sendToAll', this.state.nick, this.state.message)
      .catch(err => console.error(err));

      this.setState({message: ''});      
  };

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
          {/* <div>
            challenges:
            <ul>
            {this.props.challenges.map(function(game, index) {
               return (
               <li key={ index }>
                  {game.challengerName} has challenged you - <AcceptChallengeButton id={game.gameId} onClick={ this.aceeptChallengeOnClick.bind(this) } />
                  
                </li>
                );
            }.bind(this))}
            </ul>
          </div> */}
          <h3>User list {this.props.user.id}</h3>
          <ul>
             {this.props.connectedUsers.map(function(user, index) {
              if(user.id !== this.props.user.id) {
                  var challengeButton =  <ChallengeButton id={user.id} onClick={ this.sendChallenge.bind(this) }  />            
              }
               return ( 
               <li key={ index }>
               { challengeButton }
                { user.username }</li>
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