
import React, { useEffect, useState } from 'react';
import { Responsive, WidthProvider } from "react-grid-layout";
import Layout from './views/layout';

const ResponsiveGridLayout = WidthProvider(Responsive);

const DashBoard = ({views, componentFactory, gridProperties}) => {    
  const [isResponsive, setResponsive] = useState(0);    
  const [tiles, setTiles] = useState([]);   

  useEffect(() => { 
    async function loadTiles() {
      const generateTiles = () => {
        return views.map( view => {      
          return { view: view, mode: -1, key: view.key };
      })}

      setTiles(generateTiles());           
    }
    
    loadTiles();    
  }, [views]);  

  if (views.length == 0) { return <div>Loading ...</div> } 

  const usedTiles = tiles.filter(tile => tile.mode >= 0);
  const passiveTiles = tiles.filter(tile => tile.mode == -1);
  const gridLayout = usedTiles.map((view, i) => { return { x: (i * 2) % 12, y:1, w: 2, h:2, i: view.key };})  

  const renderTile= (tile) => {     
    const view = tile.view;
    const Component = componentFactory[view.component];  
    return (
      <div key={tile.key} className={'griditem_' + tile.mode}>
        <Layout mode={tile.mode} title={view.titel} onRefresh={() => refreshTile(tile.key)} onChangeSize={(e) => changeTileSize(e, tile.key)} onClose={() => changeTileMode(tile.key, -1)} >
          <Component key={`${tile.key}_component`} {...view.props}/>
        </Layout>  
      </div>  
    )
  }   

  const toggleIsResponsive = () => {
      setResponsive(1 - isResponsive);
  }
    
  const refreshTile = (tile) => { 
      alert("refreshView");
  }     
    
  const changeTileSize = (direction, key) => {    
    let thisTile = tiles.find(v => v.key == key);
    let newMode = thisTile.mode + direction;
    if (newMode >= 1 && newMode <= 3)
      changeTileMode(key, newMode)
  }

  const changeTileMode = (key, mode) => {        
    setTiles(
      tiles.map((tile) => tile.key === key ? 
        Object.assign({}, tile, {"mode" : mode}) : tile )
    )
  }      
  
  return (   
    <div>
      <div className={`tile sidebar ${(isResponsive)?"responsive":""}`} id="sidebar">
        { passiveTiles.map(tile => { 
          return (<a className="menuItem" key={tile.view.key} onClick={() => changeTileMode(tile.view.key, 2)}>{tile.view.titel}</a>)
        })}        
        <a href="javascript:void(0);" className="icon" onClick={() => toggleIsResponsive()}>=</a>
      </div>
      <div className="content">
        <ResponsiveGridLayout layouts={{lg: gridLayout}} {...gridProperties}> 
          { usedTiles.map(tile => { return renderTile(tile); }) }   
        </ResponsiveGridLayout>   
      </div>     
    </div>
  );
}

export default DashBoard
