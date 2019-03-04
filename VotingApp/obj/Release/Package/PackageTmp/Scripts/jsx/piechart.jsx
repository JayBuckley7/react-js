//this is what defines our piechart on the dashboard

var colors = ['#FD9827']; //this color is an artifact of me trying to make a bunch of colors, this is the reason why the app with the most apps is always orange, because its first in the array
pop(); //pop will make fill out possible colors array for us to use


function pop() {
    for (var i = 0; i < 200; i++) { // from 0, 200 so if you had 200 cramped little options all of them would have a unique...ish color you can boost it almost forever though
        colors.push(getRandomColor()); //adds the new color the the stack
    }
}
function getRandomColor() {
    //so a color is a # followed by a hex favlue so here are the hex values
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)]; //we take # and then we just append a random assortment of hex... stuff on the back yay new color
    }
    return color;
}//this comparotor is used to sort the location of to objects(say for instance... the number of votes a table has) it returns 1 or or -1
function compare(a, b) {
    if (a.count < b.count)
        return 1;
    if (a.count > b.count)
        return -1;
    return 0;
}

//here where we start thepie chart stuff
var D3Legend = React.createClass({

  propTypes: { // properties that a pie chart has
    width: React.PropTypes.number,
    height: React.PropTypes.number,//using number for height and width because circles.
    colors: React.PropTypes.array.isRequired, //this is the array we filled with color
    data: React.PropTypes.array.isRequired, //this is the json dictionary we will pass in
  },

  render: function() { //think of this like a constructor
    var color = this.props.colors;
    var data = this.props.data;
    var elements = data.map(function(item, i){
      return (
        <LegendElement color={color} xpos="0" ypos={100+i*20} data={item.name} key={i} ikey={i}/>
      )
    })

    return(
        <svg className="legend" width={this.props.width} height={this.props.height}>{elements}</svg>
    );
  }
});



var LegendElement = React.createClass({
  render: function() {
    var position =  "translate(" + this.props.xpos + "," + this.props.ypos + ")";
      //size of the table is defined in the rect width and height down there
    return (
      <g transform={position}>
        <rect width="18" height="18" fill={this.props.color[this.props.ikey]}></rect>     
        <text x="24" y="9" dy=".35em">{this.props.data}</text>
      </g>
    );
  }
});

//this peice of magic decides how big each slice of pie is
var Sector = React.createClass({
  getInitialState: function() {
    return {text: '', opacity:'arc'};
  },
  render: function() {
    var outerRadius = this.props.width/2.2;
    var innerRadius = this.props.width/8;
    var arc = d3.svg.arc()
        .outerRadius(outerRadius)
        .innerRadius(innerRadius);
    var data = this.props.data;
    var center = "translate(" + arc.centroid(data) + ")";
    var percentCenter = "translate(0,3)";
    var color = this.props.colors;
    return (
      <g onMouseOver={this.onMouseOver} onMouseOut={this.onMouseOut} onClick={this.onClick}>
        <path className={this.state.opacity} fill={colors[this.props.ikey]} d={arc(this.props.data)}></path>
        <text fill="white" transform={center} textAnchor="middle" fontSize="15px">{data.value}</text>
        <text fill={colors[this.props.ikey]} stroke={color} fontSize="15px" transform={percentCenter} textAnchor="middle">{this.state.text}</text>
      </g>
    );
  },

    //this is for the onHober function that shows % of pie on each slice
  onMouseOver: function() {
    this.setState({text: '', opacity:'arc-hover'});
    var percent = (this.props.data.value/this.props.total)*100;
    percent = percent.toFixed(1);
    this.setState({text: percent + " %"});
  },

  onMouseOut: function() {
    this.setState({text: '', opacity:'arc'});
  },
    //onClick 
  onClick: function() {
    alert("You clicked "+this.props.name);
  }
});

//////////// data series////////////
var DataSeries = React.createClass({
  propTypes: {
    width: React.PropTypes.number.isRequired,
    height: React.PropTypes.number.isRequired,
    color: React.PropTypes.array,
    data: React.PropTypes.array.isRequired,
  },
    render: function () {
    
    var color = this.props.colors;
    var data = this.props.data;
    data.sort(compare)
    var width = this.props.width;
    var height = this.props.height;
    var pie = d3.layout.pie();
    var result = data.map(function(item){
      return item.count;
    })
    var names = data.map(function(item){
      return item.name;
    })
    var sum = result.reduce(function(memo, num){ return memo + num; }, 0);
    var position = "translate(" + (width)/2 + "," + (height)/2 + ")";
    var bars = (pie(result)).map(function(point, i) {
      return (
        <Sector data={point} ikey={i} key={i} name={names[i]} colors={color} total=  
        {sum} width={width} height={height}/>
      )
    });

    return (
        <g transform={position}>{bars}</g>
    );
  }
});
//////////// end data series////////////

var D3Chart = React.createClass({
  propTypes: {
    width: React.PropTypes.number.isRequired,
    height: React.PropTypes.number.isRequired,
    children: React.PropTypes.node,
  },
  render: function() {
    return (
      <svg width={this.props.width} height={this.props.height}>        
      {this.props.children}</svg>
    );
  }
});

var D3PieChart = React.createClass({
  propTypes: {
    width: React.PropTypes.number,
    height: React.PropTypes.number,
    title: React.PropTypes.string,
    data: React.PropTypes.array.isRequired,
  },

  getDefaultProps: function() {
    return {
      width: 300,
      height: 350,
      title: '',
      Legend: true,
    };
  },

  render: function() {
 
    return (
      <div>
        <h4> {this.props.title} </h4>
        <D3Chart width={this.props.width} height={this.props.height}>
              <DataSeries data={this.props.data} colors={colors} width=
                {this.props.width} height={this.props.height}/>
        </D3Chart>
        <D3Legend data={this.props.data} colors={colors} width={this.props.width - 100} height={this.props.height} />
      </div>
    );
  }
});



