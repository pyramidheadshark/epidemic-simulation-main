using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charting
{
    public class Graph
    {
        public enum GraphType
        {
            Line,
            Fill
        }

        /// <summary>
        /// Определяет, будет ли нарисованный график залитым
        /// </summary>
        public GraphType Type { get; set; }

        /// <summary>
        /// Нижняя левая позиция графа
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Размер графа.
        /// Размер значений графа будет растянут чтобы полностью поместиться в Size.X
        /// Вертикально значения скаллирированы относительно MaxValue, а максимальные будут равны Size.Y
        /// </summary>
        public Point Size { get; set; }

        /// <summary>
        /// Определяет вертикальное скаллирование графа
        /// Значение равное максимальному значению будет отображено в верхней части графа (на Size.Y)
        /// </summary>
        public float MaxValue { get; set; }

        private Vector2 _scale = new Vector2(1.0f, 1.0f);

        BasicEffect _effect;
        short[] lineListIndices;
        short[] triangleStripIndices;

        public Graph(GraphicsDevice graphicsDevice, Point size)
        {
            _effect = new BasicEffect(graphicsDevice);
            _effect.View = Matrix.CreateLookAt(Vector3.Backward, Vector3.Zero, Vector3.Up);
            _effect.Projection = Matrix.CreateOrthographicOffCenter(0, (float)graphicsDevice.Viewport.Width, (float)graphicsDevice.Viewport.Height, 0, 1.0f, 1000.0f);
            _effect.World = Matrix.Identity;

            _effect.VertexColorEnabled = true;

            this.MaxValue = 1;
            this.Size = size;
            if (size.Y <= 0)
                Size = new Point(size.X, 1);
            if (size.X <= 0)
                Size = new Point(1, size.Y);

            this.Type = Graph.GraphType.Line;
        }

        void UpdateWorld()
        {
            _effect.World = Matrix.CreateScale(_scale.X, _scale.Y, 1.0f)
                          * Matrix.CreateRotationX(MathHelper.Pi)
                          * Matrix.CreateTranslation(new Vector3(this.Position, 0));
        }

        /// <summary>
        /// Рисует значения в указанном порядке, с указанным цветом для каждого значения
        /// </summary>
        /// <param name="values">Значение/цветные пары для рисования, в порядке слева направо</param>
        public void Draw(List<Tuple<float, Color>> values)
        {
            if (values.Count < 2)
                return;

            // Создает масштабирование (для преобразования) на основе количества точек для рисования
            float xScale = this.Size.X / (float)values.Count;
            float yScale = this.Size.Y / MaxValue;

            _scale = new Vector2(xScale, yScale);
            UpdateWorld();

            //Различные списки точек для разных типов графиков
            if (this.Type == GraphType.Line)
            {
                VertexPositionColor[] pointList = new VertexPositionColor[values.Count];
                for (int i = 0; i < values.Count; i++)
                {
                    pointList[i] = new VertexPositionColor(new Vector3(i, values[i].Item1 < this.MaxValue ? values[i].Item1 : this.MaxValue, 0), values[i].Item2);
                }

                DrawLineList(pointList);
            }
            else if (this.Type == GraphType.Fill)
            {
                VertexPositionColor[] pointList = new VertexPositionColor[values.Count * 2];
                for (int i = 0; i < values.Count; i++)
                {
                    //Вершины создаются таким образом, чтобы треугольники были перевернутыми (обращеными к зрителю). Когда они будут повернуты, они станут передними.
                    //Это делается для того, чтобы избежать изменения режима отрисовки на CullMode.CullClockwiseFace.
                    pointList[i * 2 + 1] = new VertexPositionColor(new Vector3(i, values[i].Item1 < this.MaxValue ? values[i].Item1 : this.MaxValue, 0), values[i].Item2);
                    pointList[i * 2] = new VertexPositionColor(new Vector3(i, 0, 0), values[i].Item2);
                }

                DrawTriangleStrip(pointList);
            }
        }

        /// <summary>
        /// Рисует значения в указанном порядке, в указанном цвете
        /// </summary>
        /// <param name="values">Значения для рисования, в порядке слева направо</param>
        /// <param name="color">>Цвет всего графика</param>
        public void Draw(List<float> values, Color color)
        {
            if (values.Count < 2)
                return;

            float xScale = this.Size.X / (float)values.Count;
            float yScale = this.Size.Y / MaxValue;

            _scale = new Vector2(xScale, yScale);
            UpdateWorld();

            if (this.Type == GraphType.Line)
            {
                VertexPositionColor[] pointList = new VertexPositionColor[values.Count];
                for (int i = 0; i < values.Count; i++)
                {
                    pointList[i] = new VertexPositionColor(new Vector3(i, values[i] < this.MaxValue ? values[i] : this.MaxValue, 0), color);
                }

                DrawLineList(pointList);
            }
            else if (this.Type == GraphType.Fill)
            {
                VertexPositionColor[] pointList = new VertexPositionColor[values.Count * 2];
                for (int i = 0; i < values.Count; i++)
                {
                    pointList[i * 2 + 1] = new VertexPositionColor(new Vector3(i, values[i] < this.MaxValue ? values[i] : this.MaxValue, 0), color);
                    pointList[i * 2] = new VertexPositionColor(new Vector3(i, 0, 0), color);
                }

                DrawTriangleStrip(pointList);
            }
        }

        void DrawLineList(VertexPositionColor[] pointList)
        {
            // Индексы обновляются только при изменении количества точек
            if (lineListIndices == null || lineListIndices.Length != ((pointList.Length * 2) - 2))
            {
                lineListIndices = new short[(pointList.Length * 2) - 2];
                for (int i = 0; i < pointList.Length - 1; i++)
                {
                    lineListIndices[i * 2] = (short)(i);
                    lineListIndices[(i * 2) + 1] = (short)(i + 1);
                }
            }

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _effect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList,
                    pointList,
                    0,
                    pointList.Length,
                    lineListIndices,
                    0,
                    pointList.Length - 1
                );
            }
        }

        void DrawTriangleStrip(VertexPositionColor[] pointList)
        {
            if (triangleStripIndices == null || triangleStripIndices.Length != pointList.Length)
            {
                triangleStripIndices = new short[pointList.Length];
                for (int i = 0; i < pointList.Length; i++)
                    triangleStripIndices[i] = (short)i;
            }

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _effect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    pointList,
                    0,
                    pointList.Length,
                    triangleStripIndices,
                    0,
                    pointList.Length - 2
                );
            }
        }
    }
}
