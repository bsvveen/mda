import React, { Component } from "react";
import PropTypes from 'prop-types';
import Repository from './repository';
import './view.css';

class View extends Component { 

  _isMounted = false;

  PropTypes = {   
    id: PropTypes.string.isRequired,
    controller: PropTypes.string.isRequired
  }

  state = { isLoading: true, current: {} };   

  componentDidMount() {   
    this._isMounted = true;

    this.setState({ isLoading: true });  

    this.repository = new Repository(this.props.controller); 
       
    let filter = [{ "field" : "id", "value" : this.props.id }];   
    this.repository.List(filter).then(response => {
      if (this._isMounted) {
      this.setState({ current: response[0] }, () => {
        this.setState({ isLoading: false });
      })};
    })
  }    
  
  componentWillUnmount() {
    this._isMounted = false;
  }

  Recursive = (obj) => {
    let result = [];
    for (let key in obj) {
      (typeof obj[key] === 'object') ?          
          result.push(<div className="container" key={key}><div className="cell"> { key } </div> <div className="cell"> { this.Recursive(obj[key]) } </div> </div>) :  
          (obj[key] && key !== "id" && !key.includes("_id")) ?
            result.push(<div className="row" key={key}><div> { key } </div><div> { obj[key].toString() } </div></div>) :
            result.push()
    }     
    return result;
  }

  render() { return <div className="grid"> {this.Recursive(this.state.current)} </div> }
}

export default View;