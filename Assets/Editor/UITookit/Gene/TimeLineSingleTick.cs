using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

namespace ES
{
    public class TimeLineSingleTick : VisualElement
    {
        public float nowWidth = 20;
        public float nowHeight = 60;
        public float scale = 1;
        public Color colorLine = Color.white;
        public float baseLeft = 0;
        public new class UxmlFactory : UxmlFactory<TimeLineSingleTick, UxmlTraits>
        {
        }
        public TimeLineSingleTick()
        {
            style.width = nowWidth;
            style.height = nowHeight;
            style.position = Position.Absolute;
            generateVisualContent += GeneContent;
        }
        private void GeneContent(MeshGenerationContext MGC)
        {
            var painter = MGC.painter2D;
            painter.lineWidth = nowWidth;
            painter.strokeColor = colorLine;
            painter.BeginPath();
            painter.MoveTo(Vector2.zero);
            painter.LineTo(new Vector2(0,nowHeight));
            painter.Stroke();
            Debug.Log("RePaint");
        }
    }

    public class TimeLineRuler : VisualElement
    {
        public float maxTime = 5;
        public float perSecondClip = 50;
        public int Big = 5;
        public float nowWidth = 200;
        public float nowHeight = 160;
        public float scale = 1;
        public Color colorLine = Color.white;
        public new class UxmlFactory : UxmlFactory<TimeLineRuler, UxmlTraits>
        {
        }
        public TimeLineRuler()
        {
            style.width = nowWidth;
            style.height = nowHeight;
            style.position = Position.Absolute;
            this.RegisterCallback<WheelEvent>(OnWheel);
            for (int i = 0; i < maxTime * perSecondClip; i++)
            {
             
                var tick = new TimeLineSingleTick();
                if (i% perSecondClip == 0)
                {
                    tick.nowWidth = 6;
                    tick.colorLine = Color.yellow;
                    Label label = new Label();
                    label.text = "【*" + (i / perSecondClip) + "】";
                    label.style.fontSize = 10;
                    label.style.unityTextAlign = TextAnchor.UpperLeft;
                    label.style.color = Color.yellow;
                    label.style.left = 1;
                    label.style.top = tick.nowHeight + 2;

                    tick.Add(label);

                }
                else if (i % Big == 0)
                {
                    tick.nowWidth = 6;
                    tick.colorLine = Color.white;
                    Label label = new Label();
                    label.text =  (i % perSecondClip).ToString();
                    label.style.fontSize = 10;
                    label.style.unityTextAlign = TextAnchor.UpperLeft;
                    label.style.color = Color.yellow;
                    label.style.left = 1;
                    label.style.top = tick.nowHeight + 2;
                    label.style.position = Position.Absolute;
                    tick.Add(label);
                }
                else
                {
                    tick.nowWidth = 3;
                    tick.colorLine = Color.gray;
                    tick.nowHeight = 50;
                }
                
                tick.style.left=tick.baseLeft = i * 10;
                Add(tick);
            }
        }

        private void OnWheel(WheelEvent evt)
        {
            var targetScale = scale + evt.delta.y*0.035f;
            scale = Mathf.Clamp(targetScale, 0.1f, 5);
            var All = this.Query<TimeLineSingleTick>();
            All.ForEach((tick) => { tick.style.left = scale*tick.baseLeft; });
            this.MarkDirtyRepaint();
            Debug.Log(scale);
        }

        private void GeneContent(MeshGenerationContext MGC)
        {
         
        }
    }
}
