
import React from 'react';

import SplitterLayout from 'react-splitter-layout';
import 'react-splitter-layout/lib/index.css';

import TileRender from './tilerender';
import './dashboard.css';

class Tile extends React.PureComponent {
  render() {    
      return (<div>{this.props.children}</div>)
    }
}

//https://react-cn.github.io/react/docs/create-fragment.html
class DashBoard extends React.PureComponent {

  state = { activePages: [], isMobile: false };

  componentDidMount() {
    window.addEventListener("resize", this.resize.bind(this));
    this.resize();
  }

  resize() {
    this.setState({ isMobile: window.innerWidth <= 560 });
  }

  onTileRefresh = (id) => {
    let newActivePages = Object.assign({}, this.state.activePages, { [id]: { "key": [Math.random()] } });
    this.setState({ activePages: newActivePages });
  }

  onTileDrop = (event) => {
    event.preventDefault();
    let sourceId = event.dataTransfer.getData("sourceId");
    let targetId = event.target.id;
    if (targetId && sourceId) {
      let newActivePages = Object.assign({}, this.state.activePages, { [sourceId]: { "key": [Math.random()], "pane": targetId } });
      this.setState({ activePages: newActivePages });
    }
  }

  onTileDragOver = (event) => {
    event.preventDefault();
  }

  onTileDragStart = (event, id) => {
    event.dataTransfer.setData("sourceId", id);
  } 

  filterChild = (child, pane) => {
    if (this.state.activePages[child.props.id]) {
      return (this.state.activePages[child.props.id].pane === pane)
    } else {
      return (child.props.pane === pane)
    }
  }

  render() {
    const children = React.Children.toArray(this.props.children);
    const leftPaneChildren = children.filter(child => this.filterChild(child, "left"));
    const rightPaneChildren = children.filter(child => this.filterChild(child, "right"));

    if (this.state.isMobile) {
      return (
        <div id="dashboard">
          <div id="tilescontainer">
            { children.map((child, i) => {
                return <TileRender key={i} component={child} onRefresh={this.onTileRefresh} />
            })}
          </div>
        </div>
      )
    }

    return (
      <div id="dashboard">
        <div id="tilescontainer">
          <SplitterLayout>
            <div id="left" onDrop={(e) => this.onTileDrop(e)} onDragOver={(e) => this.onTileDragOver(e)}>
              { leftPaneChildren.map((child, i) => {
                  return <TileRender key={"left" + i} component={child} onRefresh={this.onTileRefresh} onDragStart={(this.onTileDragStart)} />
              })}
            </div>
            <div id="right" onDrop={(e) => this.onTileDrop(e)} onDragOver={(e) => this.onTileDragOver(e)}>
              { rightPaneChildren.map((child, i) => {
                  return <TileRender key={"right" + i} component={child} onRefresh={this.onTileRefresh} onDragStart={(this.onTileDragStart)} />
              })}
            </div>
          </SplitterLayout>
        </div>
      </div>
    );
  }
}

export { Tile, DashBoard } 