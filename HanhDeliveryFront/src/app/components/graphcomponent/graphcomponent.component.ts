import { Component, OnInit } from '@angular/core';
import * as d3 from 'd3';
import { DeliveryServicesService } from '../../services/delivery/delivery-services.service';

interface Node {
  id: number;
  name: string;
}


@Component({
  selector: 'app-graphcomponent',
  standalone: true,
  imports: [],
  templateUrl: './graphcomponent.component.html',
  styleUrl: './graphcomponent.component.css'
})
export class GraphcomponentComponent implements OnInit {


  constructor(private deliveryServices: DeliveryServicesService){}

  ngOnInit(): void {
    // Datos del grafo
    const graph = this.deliveryServices.getGrid()
    console.log(graph.nodes.$values)
    console.log(graph.connections.$values)

    const nodeLinks = graph.connections.$values.map((l:any)=> {
      return {
        source: l.firstNodeId,
        target: l.secondNodeId
      }
    }
    )

    // Crear el contenedor SVG
    const svg = d3.select("svg");

    // Crear una fuerza de enlace
    const simulation = d3.forceSimulation(graph.nodes.$values)
      .force("link", d3.forceLink(nodeLinks).id((d:any) => d.id))
      .force("charge", d3.forceManyBody().strength(-100))
      .force("center", d3.forceCenter(400, 300));

    // Crear los enlaces
    const links = svg.selectAll("line")
      .data(nodeLinks)
      .enter()
      .append("line")
      .attr("class", "link");

    // Crear los nodos
    const nodes = svg.selectAll("circle")
      .data(graph.nodes.$values)
      .enter()
      .append("circle")
      .attr("class", "node")
      .attr("r", 2);

    // Agregar etiquetas a los nodos
    const nodeLabels = svg.selectAll("text")
      .data(graph.nodes.$values)
      .enter()
      .append("text")
      .attr("class", "node-label")
      .text((d:any) => d.name);

    // Actualizar la posición de los elementos en cada paso de simulación
    simulation.on("tick", () => {
      links
        .attr("x1", (d:any) => d.source.x)
        .attr("y1", (d:any) => d.source.y)
        .attr("x2", (d:any) => d.target.x)
        .attr("y2", (d:any) => d.target.y);

      nodes
        .attr("cx", (d:any) => d.x)
        .attr("cy", (d:any) => d.y);

      nodeLabels
        .attr("x", (d:any) => d.x)
        .attr("y", (d:any) => d.y);
    });
  }
}
