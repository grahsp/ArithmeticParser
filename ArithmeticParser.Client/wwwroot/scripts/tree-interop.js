window.treeInterop = {
    clear: function () {
        d3.select("#tree-container").selectAll("*").remove();  
    },
    render: function (elementId, data) {
        const container = document.getElementById("tree-container");
        const width = container.clientWidth;
        const height = container.clientHeight;

        const zoom = d3.zoom()
            .scaleExtent([0.2, 5])
            .on("zoom", function (event) {
                g.attr("transform", event.transform);
            });

        const initialTransform = d3.zoomIdentity
            .translate(width / 2, 125)
            .scale(1);
        
        const svg = d3
            .select("#" + elementId)
            .attr("width", width)
            .attr("height", height)
            .call(zoom);
        
        svg.selectAll("*").remove();

        const root = d3.hierarchy(data, d => d.children);

        const treeLayout = d3.tree().nodeSize([300, 150])
        treeLayout(root);

        const g = svg
            .append("g");

        
        // Links
        g.selectAll(".link")
            .data(root.links())
            .enter()
            .append("line")
            .attr("class", "link")
            .attr("x1", d => d.source.x)
            .attr("y1", d => d.source.y)
            .attr("x2", d => d.target.x)
            .attr("y2", d => d.target.y)
            .attr("stroke", "rgba(0,0,0,0.62)")
            .attr("thickness", 3);

        // Nodes
        const node = g.selectAll(".node")
            .data(root.descendants())
            .enter()
            .append("g")
            .attr("class", "node")
            .attr("transform", d => `translate(${d.x},${d.y})`);

        node.append("rect")
            .attr("x", -250/2)
            .attr("y", -25)
            .attr("width", 250)
            .attr("height", 60)
            .attr("rx", 10)
            .attr("ry", 10)
            .attr("fill", "#5a5a3f");

        node.append("text")
            .attr("text-anchor", "middle")
            .attr("fill", "#ffffff")
            .attr("font-size", 20)
            .selectAll("tspan")
            .data(d => [`Node: ${d.data.name}`, `Total: ${d.data.value}`])
            .enter()
            .append("tspan")
            .attr("x", 0)
            .attr("dy", (d, i) => i === 0 ? 0 : "1.2em")
            .text(d => d);

        
        
        svg.call(zoom.transform, initialTransform);
    }
};