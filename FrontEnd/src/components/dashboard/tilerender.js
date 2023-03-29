import React from 'react';
import PropTypes from 'prop-types';


export default class TileRender extends React.PureComponent {

  constructor(props) {
    super(props);
    this.setTileMode = this.setTileMode.bind(this);    
    this.refreshTile = this.refreshTile.bind(this);
    this.dragstart = this.dragstart.bind(this);
    
    this.state = { tileMode: "normal", refresh: true };
  }  

  static propTypes = {            
    component : PropTypes.object.isRequired
  }  

  setTileMode(newMode) {
    this.setState({ tileMode: newMode });   
  }   

  refreshTile() {
    this.setState({refresh : !this.state.refresh})
  }  

  dragstart(event) { 
    this.props.onDragStart(event, this.props.component.props.id);
  }   

  render() {    
    const mode = this.state.tileMode;
    const component = this.props.component;        

    return (
      <div className={'tile ' + mode}><div>
        <div className='actions'>
          <i onClick={this.refreshTile} className="fa-regular fa-rotate"></i>            
          { mode !== "maximized" && <i onClick={() => this.setTileMode('maximized')} className="fa-regular fa-window-maximize"></i> }
          { mode !== "normal" && <i onClick={() => this.setTileMode('normal')} className="fa-regular fa-window-restore"></i> }
          { mode !== "minimized" && <i onClick={() => this.setTileMode('minimized')} className="fa-regular fa-window-minimize"></i> }                
        </div>
        <h3 className='title' draggable="true" onDragStart={(e) => this.dragstart(e)}>{component.props.title} </h3>
        <div className="tileBody">
               
            {React.cloneElement(component, {key: this.state.refresh})}
            
          </div>      
      </div></div>
    );
  }
}