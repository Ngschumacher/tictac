import React, { Component } from 'react';

import Chat from './Chat';
import Game from './newComponents/Game';

// import logo from './logo.svg';
import './App.css';

class App extends Component {
  render() {
    return (
      <div className="App">
        {/* <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h1 className="App-title">Welcome to React</h1>
        </header> */}
       <div>
         {/* <Game verticals="3" horisontals="3" />
         <Chat /> */}

         <Game />
       </div>
      </div>
    );
  }
}

export default App;