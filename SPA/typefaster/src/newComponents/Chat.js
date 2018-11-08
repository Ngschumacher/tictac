import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';
import ChallengeButton from './ChallengeButton';
import AcceptChallengeButton from './AcceptChallengeButton';

class Chat extends Component {
  constructor(props) {
    super(props);

    this.state = {
      nick: '',
      user: {},
      message: '',
      messages: [],
      connectedUsers : [],
      challenges : [],
      hubConnection: null,
      gameInProgress : this.props.gameInProgress
    };
  }

  componentDidMount = () => {
    

      let nick = "John";
      
    // const nick = window.prompt('Your name:', 'John');
    var isChrome = !!window.chrome && !!window.chrome.webstore;
    var isFirefox = typeof InstallTrigger !== 'undefined';

    if(isChrome)
    {
      nick = "Chrome user";
    }
    if(isFirefox) 
    {
      nick = "isFirefox";
    }

    const hubConnection = new HubConnectionBuilder()
                      .withUrl('http://localhost:5000/chatHub')
                      .build();



    this.setState({ hubConnection, nick }, () => {
      this.state.hubConnection
        .start()
        .then(() => {
          console.log('Connection started!');

          this.state.hubConnection
            .invoke('signIn', this.state.nick)
            .catch(err => console.log(err));
        })
        .catch(err => console.log('Error while establishing connection :(', err));
      });


      this.state.hubConnection.on('userInformation', (user) => {
        console.log(user);
        this.setState({user : user});
      });

      this.state.hubConnection.on('sendToAll', (nick, receivedMessage) => {
        const text = `${nick}: ${receivedMessage}`;
        const messages = this.state.messages.concat([text]);
        this.setState({ messages });
      });

      this.state.hubConnection.on('connections', (connections) => {

        this.setState({connectedUsers : connections  });
        
        console.log(connections);
      });

      this.state.hubConnection.on('challengeRecieved', (message) => {
        var joined = this.state.challenges.concat(message);
        this.setState({ challenges: joined })
        console.log("challenges", this.state.challenges);
      });

     
  }


  sendChallenge(id) {
    let challengerId = this.state.user.id;
    let opponentId = id;

    fetch(`http://localhost:5000/api/game/SendChallenge?challengerId=${challengerId}&opponentId=${opponentId}`)
    .then(result => {
    })
  }
  

  acceptChallenge(id) {
    console.log("accepting game", id);
    let accepterId = this.state.user.id;
    
    fetch(`http://localhost:5000/api/game/AcceptChallenge?AccepterId=${accepterId}&gameId=${id}`)
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
          <div>
            challenges:
            <ul>
            {this.state.challenges.map(function(game, index) {
               return (
               <li key={ index }>
                  {game.username} has challenged you - <AcceptChallengeButton id={game.gameId} onClick={ this.acceptChallenge.bind(this) } />
                  
                </li>
                );
            }.bind(this))}
            </ul>
          </div>
          <h3>User list {this.state.user.id}</h3>
          <ul>
             {this.state.connectedUsers.map(function(user, index) {
              if(user.id !== this.state.user.id) {
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