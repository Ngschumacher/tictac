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

// fetch(`http://localhost:5000/api/game/SignIn?username=${nick}`)
//     .then(res => res.json())
//     .then(
//       (result) => {
//         console.log(result);
//       });
    

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
      this.state.hubConnection.on('userInformation', (user) => {
        console.log(user);
        this.setState({user : user});
      })


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
        console.log(message);
      });

    });

    
  };

  sendChallenge(id) {
    console.log("activated", id);
    this.state.hubConnection
        .invoke('sendChallenge',this.state.user.id, id)
        .catch(err => console.error(err));
  }


  acceptChallenge(id) {
    this.props.startGame(id);

    console.log(this.state.gameInProgress);
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
            {this.state.challenges.map(function(user, index) {
               return (
               <li key={ index }>
                  {user.username} has challenged you - <AcceptChallengeButton id={user.id} onClick={ this.acceptChallenge.bind(this) } />
                  
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