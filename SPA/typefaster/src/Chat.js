import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';

class Chat extends Component {
  constructor(props) {
    super(props);

    this.state = {
      nick: '',
      message: '',
      messages: [],
      connectedUsers : [],
      hubConnection: null,
    };
  }

  componentDidMount = () => {
    fetch("http://localhost:5000/api/values")
    .then(res => res.json())
    .then(
      (result) => {
        console.log(result);
      });

    // const nick = window.prompt('Your name:', 'John');
      const nick = "John";
    

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

      this.state.hubConnection.on('sendToAll', (nick, receivedMessage) => {
        const text = `${nick}: ${receivedMessage}`;
        const messages = this.state.messages.concat([text]);
        this.setState({ messages });
      });

      this.state.hubConnection.on('connections', (connections) => {

        this.setState({connectedUsers : connections  });
        
        console.log(connections);
      });

    });

    
  };

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
          <h3>User list</h3>
          <ul>
             {this.state.connectedUsers.map(function(name, index) {
               return <li key={ index }>{name}</li>;
             })}
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