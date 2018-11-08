import React, { Component } from 'react';
import  BoardVertical from './BoardVertical';
import  Cell from './Cell';

class Game extends Component {

    state = {
        test : '',
        cellObjects : [],
        xIsNext : true,
    };

    constructor(props) {
        super(props);
        this.state = {...props};
        this.state.cellObjects = [];
        console.log(this.state);
        this.onHeaderClick = this.onHeaderClick.bind(this);
    }

    createTableHeader = (onHeaderClick) => {
        let tableHeaderData = [];

        for (var x = 0; x < this.state.verticals; x++) {
                tableHeaderData.push(<div className="cell" key={x} onClick={this.onHeaderClick}>Header {x}</div>)
        }

        return <div className="heading">{tableHeaderData}</div>;
    }

    createTable = () => {
        let table  = [];
        var cellObjects = [];

        for (var x = 0; x < this.state.verticals; x++) {
            let children = []
            for(var y = 0; y < this.state.horisontals; y++) {
                var row = [];
                let key = `${y}${x}`;
                console.log(key);
                var cell = {    
                    x : x,
                    y : y,
                    key : key, 
                    color: "none",
                    taken: false,
                    value: ''
                    };
                cellObjects.push(cell);

                children.push(<Cell key={key} color={cell.color} name="cell" mark={this.currentMark} onClick={this.onClick} value={key}  />);
            }
            table.push(<div className="row" key={x}>{children}</div>);
        }

        this.state.cellObjects = cellObjects;

        console.log(this.state.cellObjects);

        return table;

    };

    onClick = () => {
        
        console.log("click");
    }

    currentMark() {
      }

    onHeaderClick = (event) => {

        let highestValue = -1;
        console.log(this.state.cellObjects.length);
        console.log(event);

        for(var i = 0; i < this.state.cellObjects.length; i++) {
            
            // if(this.state.cellObjects[i].x == )
        }

        console.log("this has been clicked");
    }

    render() {
        
        var boardSettings = [];
        console.log(this.state.verticals);

        return (
            <div>
        <div className="table">
                {this.createTable()}
        </div>

        <div>
            <BoardVertical position="0">
            </BoardVertical>
        </div>
        </div>
        );
    }

};

export default Game;

