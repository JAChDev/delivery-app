import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import * as d3 from 'd3';
import { DeliveryServicesService } from '../../services/delivery/delivery-services.service';
import { LoginService } from '../../services/login/login.service';

@Component({
  selector: 'app-graphcomponent',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './graphcomponent.component.html',
  styleUrl: './graphcomponent.component.css'
})
export class GraphcomponentComponent implements OnInit {
  graph!:any;
  nodeLinks!:any;
  nodes!:any;
  highlightedNodes: Set<number> = new Set();
  highlightedLinks: Set<string> = new Set();
  simulation!:any
  nodesGraph!:any
  links!:any
  constructor(private deliveryServices: DeliveryServicesService, private loginService: LoginService){}

  ngOnInit(): void {
    this.loadGraph();
  }

  async loadGraph():Promise<void>{
    const token:any = this.loginService.getAuthToken();
    this.graph = await this.deliveryServices.sendTokenReceiveGrid(token);

    this.nodeLinks = this.graph.connections.$values.map((l:any)=> {
      return {
        source: l.firstNodeId,
        target: l.secondNodeId
      }
    })

    this.nodes = this.graph.nodes.$values;
    const svg = d3.select("svg");

    this.simulation = d3.forceSimulation(this.nodes)
      .force("link", d3.forceLink(this.nodeLinks).id((d:any) => d.id))
      .force("charge", d3.forceManyBody().strength(-40))
      .force("y", d3.forceY(400).strength(0.1))
      .force("collide", d3.forceCollide().radius(15).strength(0.2).iterations(5))
      .force("center", d3.forceCenter(400, 300));

    this.links = svg.selectAll("line")
      .data(this.nodeLinks)
      .enter()
      .append("line")
      .attr("stroke", "#33FF98");

    this.nodesGraph = svg.selectAll("circle")
      .data(this.nodes)
      .enter()
      .append("circle")
      .attr("class", "node")
      .attr("r", 2)
      .classed("highlighted", (d: any) => this.highlightedNodes.has(d.id));

    const nodeLabels = svg.selectAll("text")
      .data(this.nodes)
      .enter()
      .append("text")
      .attr("class", "node-label")
      .text((d:any) => d.id);

    this.simulation.on("tick", () => {
      this.links
        .attr("x1", (d:any) => d.source.x)
        .attr("y1", (d:any) => d.source.y)
        .attr("x2", (d:any) => d.target.x)
        .attr("y2", (d:any) => d.target.y)        
        .classed("highlighted", (d:any) => this.highlightedLinks.has(`${d.source.id}-${d.target.id}`))
        .style("stroke", (d:any) => this.highlightedLinks.has(`${d.source.id}-${d.target.id}`) ? "red" : "#33FF98");;

      this.nodesGraph
        .attr("cx", (d:any) => d.x)
        .attr("cy", (d:any) => d.y)

      nodeLabels
        .attr("x", (d:any) => d.x)
        .attr("y", (d:any) => d.y);
    }); 
    this.simulation.on("end", ()=> {
      this.changeColorPath();
    })
  }

  changeColorPath():void {
    this.links.style("stroke", (d:any) => this.highlightedLinks.has(`${d.source.id}-${d.target.id}`) ? "red" : "#33FF98");
  }

  highlightPath(bestPath: any[]): void {
    this.highlightedNodes.clear();
    this.highlightedLinks.clear();
    bestPath.forEach(nodeId => {
      this.highlightedNodes.add(nodeId);
      console.log(this.highlightedNodes)
    });
    for (let i = 0; i < bestPath.length - 1; i++) {
      const source = bestPath[i].id;
      const target = bestPath[i + 1].id;
      this.highlightedLinks.add(`${source}-${target}`);
      this.highlightedLinks.add(`${target}-${source}`);
    }
    this.changeColorPath();
  }
  
}
